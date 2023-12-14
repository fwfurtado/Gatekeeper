using Gatekeeper.Core.ValueObjects;
using Gatekeeper.Rest.DataLayer;
using Mapster;
using MediatR;

namespace Gatekeeper.Rest.Features.Unit.List;

public record UnitListQuery(int Page, int Size) : IRequest<PagedList<Domain.Unit.Unit>>;

public class UnitListRequestHandler(
    IUnitListFetcher fetcher
    ): IRequestHandler<UnitListQuery, PagedList<Domain.Unit.Unit>>
{
    public Task<PagedList<Domain.Unit.Unit>> Handle(UnitListQuery query, CancellationToken cancellationToken)
    {
        var pagination = query.Adapt<Pagination>();
        return fetcher.FetchAsync( pagination, cancellationToken );
    }
}
