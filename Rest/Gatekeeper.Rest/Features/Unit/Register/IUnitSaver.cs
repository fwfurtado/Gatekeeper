namespace Gatekeeper.Rest.Features.Unit.Register;

public interface IUnitSaver
{
    Task<long> SaveAsync(Domain.Unit.Unit unit, CancellationToken cancellationToken);
}
