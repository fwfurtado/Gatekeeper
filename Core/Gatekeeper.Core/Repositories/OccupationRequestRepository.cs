using System.Text;
using System.Transactions;
using Dapper;
using Gatekeeper.Core.Entities;
using Gatekeeper.Shared.Database;

namespace Gatekeeper.Core.Repositories;

public class OccupationRequestRepository : IOccupationRequestRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public OccupationRequestRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<OccupationRequest?> GetRequestByIdAsync(long id, CancellationToken cancellationToken)
    {
        const string sql = @"SELECT
                                o.id,
                                o.status,
                                orp.id PeopleId,
                                orp.name,
                                orp.document,
                                orp.email,
                                orp.phone,
                                u.id UnitId,
                                u.identifier
                            FROM
                                occupation_requests AS o
                                    JOIN
                                occupation_request_people AS orp ON o.id = orp.occupation_id
                                    JOIN
                                units AS u ON u.id = o.target_unit_id
                            WHERE o.id = @id";

        using var dbConnection = _connectionFactory.CreateConnection();

        var args = new
        {
            id
        };

        var result = await dbConnection.QueryAsync<OccupationRequest, PersonalInfo, Unit, OccupationRequest>(sql,
            (request, person, unit) =>
            {
                request.People.Add(person);
                request.Unit = unit;

                return request;
            },
            param: args,
            splitOn: "PeopleId,UnitId"
        );

        if (result is null)
        {
            return null;
        }

        return result
            .GroupBy(r => r.Id)
            .Select(group =>
            {
                var requestId = group.Key;
                var first = group.First();
                var unit = first.Unit;
                var status = first.Status;
                var people = new List<PersonalInfo>();

                foreach (var request in group)
                {
                    people.AddRange(request.People);
                }

                return new OccupationRequest()
                {
                    Id = requestId,
                    Unit = unit,
                    People = people,
                    Status = status
                };
            })
            .Single();
    }

    public async Task UpdateRequestStatusAsync(OccupationRequest request, CancellationToken cancellationToken)
    {
        const string sql = "UPDATE occupation_requests SET status=@status WHERE id = @id";

        using var dbConnection = _connectionFactory.CreateConnection();

        var arguments = new
        {
            id = request.Id,
            status = Enum.GetName(request.Status),
        };

        await dbConnection.ExecuteAsync(sql, arguments);
    }

    public async Task SaveAsync(OccupationRequest request, CancellationToken cancellationToken)
    {
        using var scoped = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        const string ocuppationSql =
            "INSERT INTO occupation_requests(target_unit_id, status, requested_at) VALUES(@unitId, @status, @requestedAt) RETURNING id;";

        using var dbConnection = _connectionFactory.CreateConnection();

        var occupationSqlArgs = new
        {
            unitId = request.Unit.Id,
            status = Enum.GetName(request.Status),
            requestedAt = DateTime.UtcNow
        };

        var id = await dbConnection.ExecuteScalarAsync<long>(ocuppationSql, occupationSqlArgs);

        const string basePeopleSql =
            "INSERT INTO occupation_request_people(occupation_id, name, document, email, phone) VALUES (@occupationId, @name, @document, @email, @phone);";

        foreach (var person in request.People)
        {
            var peopleSqlArg = new
            {
                occupationId = id,
                name = person.Name,
                document = person.Document.Number,
                email = person.Email,
                phone = person.Phone
            };

            await dbConnection.ExecuteAsync(basePeopleSql, peopleSqlArg);
        }

        scoped.Complete();
    }
}