using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.ValueObjects;

namespace Gatekeeper.Core.Services;

public interface IUnitService
{
    Task<Unit> RegisterUnitAsync(RegisterUnitCommand command, CancellationToken cancellationToken);
    
    Task<Unit?> GetUnitByIdAsync(long unitId, CancellationToken cancellationToken);
    Task<PagedList<Unit>> GetAllUnits(PageRequest pageRequest, CancellationToken cancellationToken);
}