using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Gatekeeper.Core.Infra;

public class ConnectionFactory : IConnectionFactory
{

    private readonly string _connectionString;

    public ConnectionFactory(IConfiguration configuration)
    {

        _connectionString = configuration.GetValue<string>("DATABASE_CONNECTION_STRING") ??
                            throw new InvalidOperationException(
                                "DATABASE_CONNECTION_STRING environment variable is not set."
                                );

    }
    
    public DbConnection CreateConnection()
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            throw new InvalidOperationException("DATABASE_CONNECTION_STRING environment variable is not set.");
        }

        return new NpgsqlConnection(_connectionString);
    }
}