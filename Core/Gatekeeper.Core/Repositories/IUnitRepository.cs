using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Repositories;

public interface IUnitRepository
{
    Task<long> SaveAsync(Unit unit, CancellationToken cancellationToken);
    Task<bool> ExistsIdentifierAsync(string identifier, CancellationToken cancellationToken);
    Task<Unit?> GetByIdAsync(long unitId, CancellationToken cancellationToken);
}