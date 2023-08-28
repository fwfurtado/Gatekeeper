using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Repositories;

public interface IUnitRepository
{
    Task SaveAsync(Unit unit, CancellationToken cancellationToken);
    Task<bool> ExistsIdentifierAsync(string identifier, CancellationToken cancellationToken);
}