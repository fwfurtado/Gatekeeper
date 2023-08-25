using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Services;

public interface ITenantRepository
{
    Task<bool> ExistsDocumentAsync(string document, CancellationToken cancellationToken);
    Task SaveAsync(Tenant tenant, CancellationToken cancellationToken);
}