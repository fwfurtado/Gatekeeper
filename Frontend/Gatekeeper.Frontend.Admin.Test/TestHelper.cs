namespace Gatekeeper.Frontend.Admin.Test;

public static class TestHelper
{
    private static Lazy<string> RootProjectPath { get; } = new(() =>
    {
        var directoryInfo = new DirectoryInfo(AppContext.BaseDirectory);
        do
        {
            directoryInfo = directoryInfo.Parent!;
        } while (directoryInfo.Name != "Gatekeeper.Frontend.Admin.Test");

        return directoryInfo.FullName;
    });
    
    public static string MappingsPath => Path.Combine(RootProjectPath.Value, "mappings"); 
}