using System.Reflection;
using DbUp;

namespace Gatekeeper.Migration;

public class Migrator
{
    private readonly string _connectionString;

    public Migrator(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public void Migrate(string? path = null)
    {
        var migrationPath = path ?? GetMigrationPathInAssembly();
        
        EnsureMigrationPathIsValid(migrationPath);


        EnsureDatabase.For.PostgresqlDatabase(_connectionString);

        var engine =
            DeployChanges.To
                .PostgresqlDatabase(_connectionString)
                .WithScriptsFromFileSystem(migrationPath)
                .LogToConsole()
                .Build();

        var connected = engine.TryConnect(out var error);

        if (!connected)
        {
            throw new InvalidOperationException(error);
        }

        var result = engine.PerformUpgrade();

        if (!result.Successful)
        {
            throw result.Error;
        }
    }
    
    private static void EnsureMigrationPathIsValid(string path)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException($"Migrations folder {path} not found.");
        }

        var migrationFiles = Directory.GetFiles(path, "*.sql", SearchOption.TopDirectoryOnly);

        if (migrationFiles.Length == 0)
        {
            throw new FileNotFoundException("No migration files found.");
        }
    }
    
    private static string GetMigrationPathInAssembly()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var migrations = Path.Combine(Path.GetDirectoryName(assembly.Location)!, "Migrations");

        if (!Directory.Exists(migrations)) throw new DirectoryNotFoundException("Migrations folder not found.");

        return migrations;
    }
}