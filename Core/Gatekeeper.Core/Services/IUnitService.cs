using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Services;

public interface IUnitService
{
    Task<Unit> RegisterUnitAsync(RegisterUnitCommand command, CancellationToken cancellationToken);
    
    Task<Unit?> GetUnitByIdAsync(long unitId, CancellationToken cancellationToken);
    Task<Resident> RegisterResidentAsync(long unitId, RegisterResidentCommand command, CancellationToken cancellationToken);
}