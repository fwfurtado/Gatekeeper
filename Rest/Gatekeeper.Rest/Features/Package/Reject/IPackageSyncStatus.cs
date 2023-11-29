namespace Gatekeeper.Rest.Features.Package.Reject;

public interface IPackageSyncStatus
{
    public Task SyncStatus(Domain.Package.Package package, CancellationToken cancellationToken);
}