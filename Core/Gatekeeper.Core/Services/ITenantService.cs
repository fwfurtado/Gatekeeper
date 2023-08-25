using Gatekeeper.Core.Commands;

namespace Gatekeeper.Core.Services;

public interface ITenantService
{
    Task RegisterTenantAsync(RegisterTenantCommand command, CancellationToken cancellationToken);
}