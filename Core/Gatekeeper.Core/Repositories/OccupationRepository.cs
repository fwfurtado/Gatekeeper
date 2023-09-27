using System.Data;
using Dapper;
using Gatekeeper.Core.Entities;
using Gatekeeper.Shared.Database;

namespace Gatekeeper.Core.Repositories;

public class OccupationRepository : IOccupationRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public OccupationRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task SaveOccupationAsync(
        Occupation occupation,
        CancellationToken cancellationToken
    )
    {

        const string residentSql = "INSERT INTO residents (name, document) VALUES (@name, @document) RETURNING id;";

        using var connection = _connectionFactory.CreateConnection();
        
        var residentIds = new List<long>();

        foreach (var resident in occupation.Residents)
        {
            var residentSqlArgument = new
            {
                name = resident.Name,
                document = resident.Document.Number
            };

            var id = await connection.ExecuteScalarAsync<long>(residentSql, residentSqlArgument);

            residentIds.Add(id);
        }

        const string occupationSql =
            "INSERT INTO occupations (target_unit_id, startAt, endAt) VALUES (@unitId, @start, @end) RETURNING id;";

        var occupationSqlArgument = new
        {
            unitId = occupation.Unit.UnitId,
            start = occupation.Start!,
            end = occupation.End!
        };

        var occupationId = await connection.ExecuteScalarAsync<long>(occupationSql, occupationSqlArgument);

        const string occupationMemberSql =
            "INSERT INTO occupation_members (occupation_id, resident_id) VALUES (@occupationId, @residentId);";


        foreach (var residentId in residentIds)
        {
            var occupationMemberSqlArgument = new
            {
                occupationId,
                residentId
            };

            await connection.ExecuteAsync(occupationMemberSql, occupationMemberSqlArgument);
        }

        occupation.Id = occupationId;
    }
}