using Dapper;
using Gatekeeper.Core.Configurations;
using Gatekeeper.Core.Entities;
using Gatekeeper.Shared.Database;
using Microsoft.VisualBasic.CompilerServices;

namespace Gatekeeper.Core.Repositories;

public class ResidentRepository : IResidentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;


    public ResidentRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        
    }
    

    public async Task<long> SaveAsync(Resident resident, CancellationToken cancellationToken)
    {
        const string sqlCommand = "INSERT INTO residents (document, name) VALUES (@document, @name) RETURNING id;";

        var arguments = new
        {
            document = resident.Document.Number,
            name = resident.Name   
        };

        using var connection = _connectionFactory.CreateConnection();

        var id = await connection.ExecuteScalarAsync<long>(sqlCommand, arguments);

        return id;
    }
    public async Task<bool> ExistsDocumentAsync(string document, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM residents WHERE document = @document);";

        using var connection = _connectionFactory.CreateConnection();
        
        var exists = await connection.QuerySingleAsync<bool>(sql, new { document });

        return exists;
    }

    public async Task<Resident?> GetByIdAsync(long residentId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT * FROM residents WHERE id = @residentId;";

        using var connection = _connectionFactory.CreateConnection();
        
        var resident = await connection.QuerySingleOrDefaultAsync<Resident?>(sql, new { residentId });
        
        return resident;
    }


}
