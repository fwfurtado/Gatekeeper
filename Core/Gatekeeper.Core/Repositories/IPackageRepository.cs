using System.Data;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.ValueObjects;

namespace Gatekeeper.Core.Repositories;

public interface IPackageRepository
{
    Task<long> SaveAsync(Package package, CancellationToken cancellationToken);
    Task<bool> ExistsDescriptionAsync(string description, CancellationToken cancellationToken);
    Task<Package?> GetByIdAsync(long packageId, CancellationToken cancellationToken);
    Task<PagedList<Package>> GetAll(PageRequest pageRequest, CancellationToken cancellationToken);
    Task UpdateStatus(long packageId, PackageStatus status, CancellationToken cancellationToken);
    Task DeleteByIdAsync(long packageId, CancellationToken cancellationToken);
}