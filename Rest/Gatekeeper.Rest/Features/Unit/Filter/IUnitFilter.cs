namespace Gatekeeper.Rest.Features.Unit.Filter;

public interface IUnitFilter
{
    Task<IEnumerable<Domain.Unit.Unit?>> FilterByIdentifierAsync(string identifier, CancellationToken cancellationToken);
    Task<IEnumerable<Domain.Unit.Unit?>> GetTenFirstUnitsAsync(CancellationToken cancellationToken); 
}