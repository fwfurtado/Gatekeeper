namespace Gatekeeper.Rest.Features.Package.Remove;

public interface IPackageRemover
{
    Task RemoveAsync(long id, CancellationToken cancellationToken);
}