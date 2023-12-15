using Gatekeeper.Rest.Domain.Package;

namespace Gatekeeper.Rest.Features.Package.Receive;

public interface IPackageSaver
{
    Task<PackageReceived> SaveAsync(Domain.Package.Package package, CancellationToken cancellationToken);
}