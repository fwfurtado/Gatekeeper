using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Repositories;

public interface IResidentRepository
{
    Task<bool> ExistsDocumentAsync(string document, CancellationToken cancellationToken);
    Task SaveAsync(Resident resident, CancellationToken cancellationToken);
}