namespace Gatekeeper.Rest.Features.Package.Receive;

public interface IPackageFetcherByDescription
{
    Task<bool> ExistsDescriptionAsync(string description, CancellationToken cancellationToken);
}