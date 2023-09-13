using Gatekeeper.Migration;
using Gatekeeper.Shared.Database;
using Npgsql;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;

namespace Gatekeeper.Shared.Test;

[SetUpFixture]
public abstract class DatabaseTest
{
    private readonly PostgreSqlContainer _databaseContainer = new PostgreSqlBuilder()
        .WithUsername("test-username")
        .WithPassword("test-password")
        .WithDatabase("test")
        .Build();
    
    private Respawner? _respawner;
    
    [OneTimeSetUp]
    public async Task BeforeAll()
    {
        await _databaseContainer.StartAsync();
        
        var migrator = new Migrator(_databaseContainer.GetConnectionString());
        migrator.Migrate();

        await using var connection = GetConnection();

        await connection.OpenAsync();
        _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            SchemasToInclude = new[] { "public" },
            TablesToIgnore = new[] { new Table("schemaversions") },
            DbAdapter = DbAdapter.Postgres,
        }); 
    }
    
    [SetUp]
    public async Task BeforeEach()
    {
        await using var connection = GetConnection();

        await connection.OpenAsync();
        var resetTask = _respawner?.ResetAsync(connection);
        
        if (resetTask is not null)
        {
            await resetTask;
        }
    }
    
    [OneTimeTearDown]
    public async Task AfterAll()
    {
        await _databaseContainer.StopAsync();
    }
    
    protected string ConnectionString => _databaseContainer.GetConnectionString();
    private NpgsqlConnection GetConnection() => new(ConnectionString);

    protected abstract IDbConnectionFactory GetConnectionFactory();
}