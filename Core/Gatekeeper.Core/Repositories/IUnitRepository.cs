using System.Data;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.ValueObjects;

namespace Gatekeeper.Core.Repositories;

public interface IUnitRepository
{
    Task<long> SaveAsync(Unit unit, CancellationToken cancellationToken);
    Task<bool> ExistsIdentifierAsync(string identifier, CancellationToken cancellationToken);
    Task<Unit?> GetByIdAsync(long unitId, CancellationToken cancellationToken);
    
    Task UpdateOccupationAsync(Unit unit, CancellationToken cancellationToken);
    Task<Unit?> GetByIdentifier(string identifier, CancellationToken cancellationToken);
    Task<PagedList<Unit>> GetAll(PageRequest pageRequest, CancellationToken cancellationToken);
}