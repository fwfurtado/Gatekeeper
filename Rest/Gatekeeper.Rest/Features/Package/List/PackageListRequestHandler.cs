using Gatekeeper.Core.ValueObjects;
using Gatekeeper.Rest.DataLayer;
using Mapster;
using MediatR;

namespace Gatekeeper.Rest.Features.Package.List;

public record PackageListQuery(int Page, int Size) : IRequest<PagedList<Domain.Package>>;

public class PackageListRequestHandler(
    IPackageListFetcher fetcher
) : IRequestHandler<PackageListQuery, PagedList<Domain.Package>>
{
    public Task<PagedList<Domain.Package>> Handle(PackageListQuery query, CancellationToken cancellationToken)
    {
        var pagination = query.Adapt<Pagination>();

        return fetcher.FetchAsync(pagination, cancellationToken);
    }
}