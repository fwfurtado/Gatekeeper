using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Repositories;

public interface ITenantRepository
{
    Task<bool> ExistsDocumentAsync(string document, CancellationToken cancellationToken);
    Task SaveAsync(Tenant tenant, CancellationToken cancellationToken);
}