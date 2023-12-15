using MediatR;

namespace Gatekeeper.Rest.Features.Unit.Show;

public record UnitShowQuery(long Id) : IRequest<Domain.Unit.Unit?>;

public class UnitShowRequestHandler(
    IUnitFetcherById fetcherById
    ) : IRequestHandler<UnitShowQuery, Domain.Unit.Unit?>
{
    public Task<Domain.Unit.Unit?> Handle(UnitShowQuery query, CancellationToken cancellationToken)
    {
        return fetcherById.FetchAsync(query.Id, cancellationToken);
    }
}
