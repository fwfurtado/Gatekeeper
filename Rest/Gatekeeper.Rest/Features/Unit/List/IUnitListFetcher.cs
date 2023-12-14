using Gatekeeper.Core.ValueObjects;
using Gatekeeper.Rest.DataLayer;

namespace Gatekeeper.Rest.Features.Unit.List;

public interface IUnitListFetcher
{
    Task<PagedList<Domain.Unit.Unit>> FetchAsync(Pagination pagination, CancellationToken cancellationToken);
}
