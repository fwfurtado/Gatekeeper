using Dapper;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Infra;

namespace Gatekeeper.Core.Repositories;

public class ResidentRepository : IResidentRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public ResidentRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task SaveAsync(Resident resident, CancellationToken cancellationToken)
    {
        const string sqlCommand = "INSERT INTO residents (document, name) VALUES (@document, @name);";

        var dbConnection = _connectionFactory.CreateConnection();

        var arguments = new
        {
            document = resident.Document.Number,
            name = resident.Name   
        };

        var affected = await dbConnection.ExecuteAsync(sqlCommand, arguments);

        if (affected == 0)
        {
            throw new InvalidOperationException("Resident not saved");
        }
    }
    public Task<bool> ExistsDocumentAsync(string document, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM residents WHERE document = @document);";

        var dbConnection = _connectionFactory.CreateConnection();

        return dbConnection.QuerySingleAsync<bool>(sql, new { document });
    }

    public Task<Resident?> GetByIdAsync(long residentId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT * FROM residents WHERE id = @residentId;";

        var dbConnection = _connectionFactory.CreateConnection();

        return dbConnection.QuerySingleOrDefaultAsync<Resident?>(sql, new { residentId });
    }


}
