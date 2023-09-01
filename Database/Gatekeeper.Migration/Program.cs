// See https://aka.ms/new-console-template for more information

using System.Reflection;
using DbUp;
using static System.Console;

try
{
    if (args.Length == 0)
    {
        throw new ArgumentException("Connection string and Migrations path is required.");
    }

    var connectionString =
        args[0]
        ?? throw new ArgumentException("Connection string is required.");

    var migrationPath =
        args.Length > 1
            ? args[1]
            : GetMigrationPathInAssembly();


    EnsureMigrationPathIsValid(migrationPath);


    EnsureDatabase.For.PostgresqlDatabase(connectionString);

    var engine =
        DeployChanges.To
            .PostgresqlDatabase(connectionString)
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

    PrintSuccess("Done");

    return 0;
}
catch (Exception ex)
{
    PrintError(ex.Message);
    return -1;
}

void EnsureMigrationPathIsValid(string s)
{
    if (!Directory.Exists(s))
    {
        throw new DirectoryNotFoundException($"Migrations folder {s} not found.");
    }

    var migrationFiles = Directory.GetFiles(s, "*.sql", SearchOption.TopDirectoryOnly);

    if (migrationFiles.Length == 0)
    {
        throw new FileNotFoundException("No migration files found.");
    }
}

string GetMigrationPathInAssembly()
{
    var assembly = Assembly.GetExecutingAssembly();
    var migrations = Path.Combine(Path.GetDirectoryName(assembly.Location)!, "Migrations");

    if (!Directory.Exists(migrations)) throw new DirectoryNotFoundException("Migrations folder not found.");

    PrintSuccess("Migrations folder already exists.");

    return migrations;
}

void PrintError(string message)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine(message);
    ResetColor();
}

void PrintSuccess(string message)
{
    ForegroundColor = ConsoleColor.Green;
    WriteLine(message);
    ResetColor();
}