using Gatekeeper.Core.ValueObjects;
using Gatekeeper.Rest.DataLayer;

namespace Gatekeeper.Rest.Features.Package.List;

public interface IPackageListFetcher
{
    Task<PagedList<Domain.Package>> FetchAsync(Pagination pagination, CancellationToken cancellationToken);
}