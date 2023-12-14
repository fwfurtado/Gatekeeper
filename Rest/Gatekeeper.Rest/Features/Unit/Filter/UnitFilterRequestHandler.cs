using MediatR;

namespace Gatekeeper.Rest.Features.Unit.Filter;

public record UnitFilterQuery(string Identifier) : IRequest<IEnumerable<Domain.Unit.Unit?>>;

public class UnitFilterByRequestHandler(
    IUnitFilter unitFilter
    ) : IRequestHandler<UnitFilterQuery, IEnumerable<Domain.Unit.Unit?>>
{
    public Task<IEnumerable<Domain.Unit.Unit?>> Handle(UnitFilterQuery query, CancellationToken cancellationToken)
    {
        if (query.Identifier is null)
        {
            return unitFilter.GetTenFirstUnitsAsync(cancellationToken);
        }
        else 
        {
            return unitFilter.FilterByIdentifierAsync(query.Identifier, cancellationToken);
        }
    }
}