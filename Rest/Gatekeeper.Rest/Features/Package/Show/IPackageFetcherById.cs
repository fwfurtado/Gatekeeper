namespace Gatekeeper.Rest.Features.Package.Show;

public interface IPackageFetcherById
{
    Task<Domain.Package.Package?> FetchAsync(long id, CancellationToken cancellationToken);
}