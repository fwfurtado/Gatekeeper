namespace Gatekeeper.Rest.Features.Package.Receive;

public interface IReceiveRepository
{
    Task<bool> ExistsDescriptionAsync(string description, CancellationToken cancellationToken);
    Task<long> SaveAsync(Domain.Package package, CancellationToken cancellationToken);
}