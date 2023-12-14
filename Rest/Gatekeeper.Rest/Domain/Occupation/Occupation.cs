using System.Collections.Immutable;
using Gatekeeper.Core.Entities;
using Stateless;

namespace Gatekeeper.Rest.Domain.Occupation;

public class Occupation
{
    public long Id { get; set; }
    public List<Resident> Residents { get; set; } = new();
    public required TargetUnit Unit { get; set; }
    public DateOnly? Start { get; set; }
    public DateOnly? End { get; set; }
    public OccupationStatus Status { get; set; } = OccupationStatus.Pending;

    private readonly SortedList<DateTime, IOccupationEvent> _events = new();
    public IImmutableList<IOccupationEvent> Events => _events.Values.ToImmutableList();

    public void AddEvent<T>(T occupationEvent) where T : IOccupationEvent
    {
        var stateMachine = new OccupationStateMachine(this, SetStatus);

        switch (occupationEvent)
        {
            case OccupationApproved:
                stateMachine.Approve();
                break;
            case OccupationRejected:
                stateMachine.Reject();
                break;
        }

        _events.Add(occupationEvent.OccurredAt, occupationEvent);
    }

    private void SetStatus(OccupationStatus status)
    {
        Status = status;
    }
}

file class OccupationStateMachine
{
    private readonly StateMachine<OccupationStatus, OccupationTrigger> _stateMachine;

    public OccupationStateMachine(Occupation occupation, Action<OccupationStatus> setStatus)
    {
        _stateMachine = new StateMachine<OccupationStatus, OccupationTrigger>(() => occupation.Status, setStatus);

        _stateMachine.Configure(OccupationStatus.Pending)
            .Permit(OccupationTrigger.Approve, OccupationStatus.Approved)
            .Permit(OccupationTrigger.Reject, OccupationStatus.Rejected);
    }

    public void Approve()
    {
        _stateMachine.Fire(OccupationTrigger.Approve);
    }

    public void Reject()
    {
        _stateMachine.Fire(OccupationTrigger.Reject);
    }
}

file enum OccupationTrigger
{
    Approve,
    Reject
}

public class TargetUnit
{
    public string Identifier { get; set; }
    public long UnitId { get; set; }
}
