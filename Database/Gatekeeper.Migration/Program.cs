// See https://aka.ms/new-console-template for more information

using System.Reflection;
using DbUp;
using static System.Console;

if (args.Length == 0)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine("Connection string and Migrations path is required.");
    ResetColor();
    return -1;
}


var connectionString =
    args[0]
    ?? throw new ArgumentException("Connection string is required.");

var migrationPath =
    args[1]
    ?? throw new ArgumentException("Migration path is required.");

EnsureDatabase.For.PostgresqlDatabase(connectionString);

var engine =
    DeployChanges.To
        .PostgresqlDatabase(connectionString)
        // .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => s.EndsWith(".sql"))
        .WithScriptsFromFileSystem(migrationPath)
        .LogToConsole()
        .Build();

var connected = engine.TryConnect(out var error);

if (!connected)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine(error);
    ResetColor();
    return -1;
}

var result = engine.PerformUpgrade();

if (!result.Successful)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine(result.Error);
    ResetColor();
#if DEBUG
    ReadLine();
#endif
    return -1;
}

ForegroundColor = ConsoleColor.Green;
WriteLine("Success!");
ResetColor();

return 0;