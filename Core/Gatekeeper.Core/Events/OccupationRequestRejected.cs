using MediatR;

namespace Gatekeeper.Core.Events;

public class OccupationRequestRejected : INotification
{
    public required long OccupationId { get; set; }
    public DateTime OccuredAt { get; } = DateTime.UtcNow;
    public string Reason { get; set; } = null!;
}