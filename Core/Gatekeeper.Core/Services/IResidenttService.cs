using Gatekeeper.Core.Commands;

namespace Gatekeeper.Core.Services;

public interface IResidenttService
{
    Task RegisterTenantAsync(RegisterResidentCommand command, CancellationToken cancellationToken);
}