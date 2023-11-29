using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.ValueObjects;

namespace Gatekeeper.Core.Services;

public interface IPackageService
{
    Task<Package?> GetPackageByIdAsync(long packageId, CancellationToken cancellationToken);
    Task<PagedList<Package>> GetAllPackages(PageRequest pageRequest, CancellationToken cancellationToken);
    Task DeletePackage(long packageId, CancellationToken cancellationToken);
    Task UpdateStatusDeliveredAsync(long  packageId, CancellationToken cancellationToken);
    Task UpdateStatusRejectedAsync(long packageId, CancellationToken cancellationToken);
}
