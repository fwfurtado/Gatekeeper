using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Services;

public interface IResidentService
{
    Task<Resident> RegisterResidentAsync(RegisterResidentCommand command, CancellationToken cancellationToken);

    Task<Resident?> GetResidentByIdAsync(long residentId, CancellationToken cancellationToken);
    Task<IEnumerable<Resident>> GetAllResidents(CancellationToken cancellationToken);
}