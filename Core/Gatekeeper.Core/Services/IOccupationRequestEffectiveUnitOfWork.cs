using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Services;

public interface IOccupationRequestEffectiveUnitOfWork
{
    public Task CreateOccupationAndAssociateWithUnit(Occupation occupation, Unit unit,
        CancellationToken cancellationToken);
}