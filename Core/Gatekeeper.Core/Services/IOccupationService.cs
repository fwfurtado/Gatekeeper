namespace Gatekeeper.Core.Services;

public interface IOccupationService
{
    Task ApproveRequestAsync(long requestId, CancellationToken cancellationToken);
    Task RejectRequestAsync(long requestId, string reason, CancellationToken cancellationToken);
    Task EffectiveApprovedRequest(long requestId, CancellationToken cancellationToken);
}