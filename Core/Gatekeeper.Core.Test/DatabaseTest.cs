using System.Data;
using Gatekeeper.Core.Configurations;
using Gatekeeper.Migration;
using Npgsql;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;

namespace Gatekeeper.Core.Test;

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
        _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions()
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
    
    
    private NpgsqlConnection GetConnection() => new(_databaseContainer.GetConnectionString());
    
    protected IDbConnectionFactory GetConnectionFactory() => new TestConnectionFactory(GetConnection);


    private class TestConnectionFactory : IDbConnectionFactory
    {
        private Func<IDbConnection> _factory;

        public TestConnectionFactory(Func<IDbConnection> factory)
        {
            _factory = factory;
        }

        public IDbConnection CreateConnection()
        {
            return _factory();
        }
    }
}