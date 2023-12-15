namespace Gatekeeper.Rest.Features.Unit.Show;

public interface IUnitFetcherById
{
    Task<Domain.Unit.Unit?> FetchAsync(long id, CancellationToken cancellationToken);
}
