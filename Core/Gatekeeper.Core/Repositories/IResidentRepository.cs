using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Repositories;

public interface IResidentRepository
{
    Task SaveAsync(Resident resident, CancellationToken cancellationToken);
    Task<bool> ExistsDocumentAsync(string document, CancellationToken cancellationToken);
    Task<Resident?> GetByIdAsync(long residentId, CancellationToken cancellationToken);
}