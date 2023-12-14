using Gatekeeper.Rest.Domain.Aggregate;
using Stateless;

namespace Gatekeeper.Rest.Domain.Occupation;

public class OccupationService
{
    public void Approve(Occupation occupation)
    {
        var sateMachine = new OccupationStateMachine(occupation);
        sateMachine.Approve();

        var packageId = occupation.Id!;

        occupation.AddEvent(new OccupationApproved(packageId));
    }

    public void Reject(Occupation occupation, string reason)
    {
        var sateMachine = new OccupationStateMachine(occupation);
        sateMachine.Reject();

        var occupationId = occupation.Id!;

        occupation.AddEvent(new OccupationRejected(occupationId, reason));
    }

}

file class OccupationStateMachine
{
    private readonly StateMachine<OccupationStatus, OccupationTrigger> _stateMachine;

    public OccupationStateMachine(Occupation occupation)
    {
        _stateMachine = new StateMachine<OccupationStatus, OccupationTrigger>(occupation.Status);

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
