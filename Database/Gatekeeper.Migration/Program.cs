// See https://aka.ms/new-console-template for more information

using Gatekeeper.Migration;
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
            : null;


    var migrator = new Migrator(connectionString);
    
    migrator.Migrate(migrationPath);

    PrintSuccess("Done");

    return 0;
}
catch (Exception ex)
{
    PrintError(ex.Message);
    return -1;
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