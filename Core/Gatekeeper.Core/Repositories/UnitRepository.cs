using System.Data;
using Dapper;
using Gatekeeper.Core.Configurations;
using Gatekeeper.Core.Entities;
using Microsoft.Extensions.DependencyInjection;

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
        const string sql = "SELECT * FROM units WHERE id = @unitId;";

        using var dbConnection = _connectionFactory.CreateConnection();

        var unit = await dbConnection.QuerySingleOrDefaultAsync<Unit?>(sql, new { unitId });

        return unit;
    }
}