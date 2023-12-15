using Gatekeeper.Core.Repositories;

namespace Gatekeeper.Rest.Domain.Occupation;

public interface IOccupationEvent
{
    long OccupationId { get; }
    DateTime OccurredAt { get; }
}

public record OccupationApproved(long OccupationId) : IOccupationEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

public record OccupationRejected(long OccupationId, string Reason) : IOccupationEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}