using System.Data;
using Gatekeeper.Shared.Database;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Gatekeeper.Core.Configurations;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetValue<string>("DATABASE_CONNECTION_STRING") ?? 
                            throw new InvalidOperationException("DATABASE_CONNECTION_STRING not found.");
    }
    
    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}