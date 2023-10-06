using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Repositories;

public interface IResidentRepository
{
    Task<long> SaveAsync(Resident resident, CancellationToken cancellationToken);
    Task<bool> ExistsDocumentAsync(string document, CancellationToken cancellationToken);
    Task<Resident?> GetByIdAsync(long residentId, CancellationToken cancellationToken);
    Task<IEnumerable<Resident>> GetAll(CancellationToken cancellationToken);
    Task DeleteByIdAsync(long residentId, CancellationToken cancellationToken);
}