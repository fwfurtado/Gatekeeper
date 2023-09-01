using Dapper;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Infra;

namespace Gatekeeper.Core.Repositories;

public class UnitRepository : IUnitRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public UnitRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task SaveAsync(Unit unit, CancellationToken cancellationToken)
    {
        const string sqlCommand = "INSERT INTO units (identifier) VALUES (@identifier);";

        var dbConnection = _connectionFactory.CreateConnection();
        
        var arguments = new 
        {
            identifier = unit.Identifier
        };
        
        var affected = await dbConnection.ExecuteAsync(sqlCommand, arguments);

        if (affected == 0)
        {
            throw new InvalidOperationException("Unit not saved");
        }
        
    }

    public Task<bool> ExistsIdentifierAsync(string identifier, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM units WHERE identifier = @identifier);";
        
        var dbConnection = _connectionFactory.CreateConnection();
        
        return dbConnection.QuerySingleAsync<bool>(sql, new { identifier });
    }

    public Task UpdateAsync(Unit unit, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Unit?> GetByIdAsync(long unitId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT * FROM units WHERE id = @unitId;";
        
        var dbConnection = _connectionFactory.CreateConnection();
        
        return dbConnection.QuerySingleOrDefaultAsync<Unit?>(sql, new { unitId });
    }
}