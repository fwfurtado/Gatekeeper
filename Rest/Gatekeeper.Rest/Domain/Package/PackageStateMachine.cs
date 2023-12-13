using Gatekeeper.Rest.Domain.Aggregate;
using Gatekeeper.Rest.Infra;
using MediatR;
using Stateless;

namespace Gatekeeper.Rest.Domain.Package;

public class PackageStateMachine
{
    private readonly StateMachine<PackageStatus, PackageTrigger> _stateMachine;

    private readonly StateMachine<PackageStatus, PackageTrigger>.TriggerWithParameters<PackageDelivered,
            CancellationToken>
        _deliverTrigger;

    private readonly StateMachine<PackageStatus, PackageTrigger>.TriggerWithParameters<PackageRejected,
            CancellationToken>
        _rejectTrigger;


    private readonly IDateTimeProvider _dateTimeProvider;

    private enum PackageTrigger
    {
        Deliver,
        Reject
    }

    public PackageStateMachine(PackageStatus initial, IPublisher publisher, IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _stateMachine = new StateMachine<PackageStatus, PackageTrigger>(initial);

        _rejectTrigger = _stateMachine.SetTriggerParameters<PackageRejected, CancellationToken>(PackageTrigger.Reject);
        _deliverTrigger =
            _stateMachine.SetTriggerParameters<PackageDelivered, CancellationToken>(PackageTrigger.Deliver);

        _stateMachine.Configure(PackageStatus.Pending)
            .Permit(PackageTrigger.Deliver, PackageStatus.Delivered)
            .Permit(PackageTrigger.Reject, PackageStatus.Rejected);

        _stateMachine.Configure(PackageStatus.Delivered)
            .OnEntryFromAsync(_deliverTrigger, publisher.Publish);


        _stateMachine.Configure(PackageStatus.Rejected)
            .OnEntryFromAsync(_rejectTrigger, publisher.Publish);
    }


    public async Task DeliverAsync(Package package, CancellationToken cancellationToken)
    {
        var occurredAt = _dateTimeProvider.UtcNow;
        var @event = new PackageDelivered(package.Id, occurredAt, package.Status);
        await _stateMachine.FireAsync(_deliverTrigger, @event, cancellationToken);

        package.AddEvent(@event);
    }


    public async Task RejectAsync(Package package, string reason, CancellationToken cancellationToken)
    {
        var occurredAt = _dateTimeProvider.UtcNow;
        var @event = new PackageRejected(package.Id, occurredAt, package.Status, reason);
        await _stateMachine.FireAsync(_rejectTrigger, @event, cancellationToken);

        package.AddEvent(@event);
    }
}