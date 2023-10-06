using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Services;

public interface IUnitService
{
    Task<Unit> RegisterUnitAsync(RegisterUnitCommand command, CancellationToken cancellationToken);
    
    Task<Unit?> GetUnitByIdAsync(long unitId, CancellationToken cancellationToken);
    Task<IEnumerable<Unit>> GetAllUnits(CancellationToken cancellationToken);
}