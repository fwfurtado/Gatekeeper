namespace Gatekeeper.Core.Services;

public interface IImportCsvService
{
    Task ImportCsv(string filePath, CancellationToken cancellationToken);
}
