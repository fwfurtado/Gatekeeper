namespace Gatekeeper.Rest.Features.Package.Receive;

public interface IPackageSaver
{
    Task<long> SaveAsync(Domain.Package.Package package, CancellationToken cancellationToken);
}