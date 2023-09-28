using MediatR;

namespace Gatekeeper.Core.Events;

public class OccupationRequestApproved: INotification
{
    public required long OccupationId { get; set; }
    public DateTime OccuredAt { get; } = DateTime.UtcNow;
}