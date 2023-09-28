using Gatekeeper.Core.Services;
using MediatR;

namespace Gatekeeper.Core.Events.Handlers;

public class OccupationRequestApprovedHandler : INotificationHandler<OccupationRequestApproved>
{
    private readonly IOccupationService _service;

    public OccupationRequestApprovedHandler(IOccupationService service)
    {
        _service = service;
    }

    public Task Handle(OccupationRequestApproved notification, CancellationToken cancellationToken)
    {
        var occupationId = notification.OccupationId;
        
        return _service.EffectiveApprovedRequest(occupationId, cancellationToken);
    }
}