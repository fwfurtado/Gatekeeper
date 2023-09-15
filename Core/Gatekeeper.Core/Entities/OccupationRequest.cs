using Gatekeeper.Core.Configurations;
using Gatekeeper.Core.Events;
using Stateless;

namespace Gatekeeper.Core.Entities;

public class OccupationRequest
{
    public long Id { get; set; }
    public required Unit Unit { get; init; }
    public required List<PersonalInfo> People { get; init; }
    public OccupationRequestStatus Status { get; private set; }
    
    public string? RejectReason { get; private set; }
    public bool IsNotApproved => Status != OccupationRequestStatus.Approved;

    private readonly OccupationRequestStateMachine _stateMachine;


    public OccupationRequest(Unit unit, List<PersonalInfo> people)
    {
        People = people;
        Unit = unit;
        _stateMachine = new OccupationRequestStateMachine(this);
    }
    
    public OccupationRequestApproved Approve()
    {
        _stateMachine.Fire(OccupationRequestTriggers.Approve);

        return new OccupationRequestApproved
        {
            OccupationId = Id
        };
    }

    public OccupationRequestRejected Reject(string reason)
    {
        _stateMachine.Fire(OccupationRequestTriggers.Reject);
        
        RejectReason = reason;
        
        return new OccupationRequestRejected
        {
            OccupationId = Id,
            Reason = reason
        };
    }
    private sealed class OccupationRequestStateMachine : StateMachine<OccupationRequestStatus, OccupationRequestTriggers>
    {
        public OccupationRequestStateMachine(OccupationRequest request) : base(() => request.Status, newStatus => request.Status = newStatus)
        {
            Configure(OccupationRequestStatus.Pending)
                .Permit(OccupationRequestTriggers.Approve, OccupationRequestStatus.Approved)
                .Permit(OccupationRequestTriggers.Reject, OccupationRequestStatus.Rejected);
        }
    }
    
    private enum OccupationRequestTriggers
    {
        Approve,
        Reject,
    }
}