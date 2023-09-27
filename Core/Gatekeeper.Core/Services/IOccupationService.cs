using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Services;

public interface IOccupationService
{
    Task ApproveRequestAsync(long requestId, CancellationToken cancellationToken);
    Task RejectRequestAsync(long requestId, string reason, CancellationToken cancellationToken);
    Task EffectiveApprovedRequest(long requestId, CancellationToken cancellationToken);
    Task RequestOccupationAsync(NewOccupationCommand command, CancellationToken cancellationToken);
    Task<OccupationRequest?> GetById(long id, CancellationToken cancellationToken);
}