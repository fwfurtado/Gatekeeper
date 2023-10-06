using System.Data;
using Dapper;
using Gatekeeper.Core.Entities;
using Gatekeeper.Shared.Database;

namespace Gatekeeper.Core.Repositories;

public class UnitRepository : IUnitRepository
{
    private readonly IDbConnectionFactory _connectionFactory;


    public UnitRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        
    }

    public async Task<long> SaveAsync(Unit unit, CancellationToken cancellationToken)
    {
        const string sqlCommand = "INSERT INTO units (identifier) VALUES (@identifier) RETURNING id;";

        using var dbConnection = _connectionFactory.CreateConnection();

        var arguments = new
        {
            identifier = unit.Identifier
        };

        var id = await dbConnection.ExecuteScalarAsync<long>(sqlCommand, arguments);


        return id;
    }

    public async Task<bool> ExistsIdentifierAsync(string identifier, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM units WHERE identifier = @identifier);";

        using var dbConnection = _connectionFactory.CreateConnection();

        var exists = await dbConnection.QuerySingleAsync<bool>(sql, new { identifier });

        return exists;
    }

    public async Task<Unit?> GetByIdAsync(long unitId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT id, identifier FROM units WHERE id = @unitId;";

        using var dbConnection = _connectionFactory.CreateConnection();

        var unit = await dbConnection.QuerySingleOrDefaultAsync<Unit?>(sql, new { unitId });

        return unit;
    }

    public async Task UpdateOccupationAsync(Unit unit, CancellationToken cancellationToken)
    {
        const string sql = "UPDATE units SET occupation_id = @occupationId WHERE id = @unitId;";
        
        using var connection = _connectionFactory.CreateConnection();

        if (unit.Occupation is null)
        {
            throw new ArgumentException("Unit must have an occupation");
        }
        
        var arguments = new
        {
            occupationId = unit.Occupation.Id,
            unitId = unit.Id
        };
        
        await connection.ExecuteAsync(sql, arguments);
    }

    public async Task<Unit?> GetByIdentifier(string identifier, CancellationToken cancellationToken)
    {
        const string sql = "SELECT id, identifier FROM units WHERE identifier = @identifier;";

        using var dbConnection = _connectionFactory.CreateConnection();

        var unit = await dbConnection.QuerySingleOrDefaultAsync<Unit?>(sql, new { identifier });

        return unit;
    }

    public async Task<IEnumerable<Unit>> GetAll(CancellationToken cancellationToken)
    {
        const string sql = "SELECT id, identifier FROM units";
        
        using var dbConnection = _connectionFactory.CreateConnection();
        
        var units = await dbConnection.QueryAsync<Unit>(sql);
        
        return units;
    }
}