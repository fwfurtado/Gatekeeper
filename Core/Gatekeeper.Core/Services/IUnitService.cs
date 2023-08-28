using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Services;

public interface IUnitService
{
    Task<Unit> RegisterUnitAsync(RegisterUnitCommand command, CancellationToken cancellationToken);
}