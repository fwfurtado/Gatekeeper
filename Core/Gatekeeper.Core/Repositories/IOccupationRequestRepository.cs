using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Repositories;

public interface IOccupationRequestRepository
{
    Task<OccupationRequest?> GetRequestByIdAsync(long id, CancellationToken cancellationToken);
    
    Task UpdateRequestStatusAsync(OccupationRequest request, CancellationToken cancellationToken);

    Task SaveAsync(OccupationRequest request, CancellationToken cancellationToken);
}