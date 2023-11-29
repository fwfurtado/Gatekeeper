using System.Collections.Immutable;
using Gatekeeper.Rest.Domain.Aggregate;
using Stateless;

namespace Gatekeeper.Rest.Domain.Package;

public class Package
{
    public long? Id { get; init; } = null;
    public required string Description { get; init; }

    public DateTime ArrivedAt { get; } = DateTime.UtcNow;

    public required long UnitId { get; init; }

    public PackageStatus Status { get; private set; } = PackageStatus.Pending;


    private readonly SortedList<DateTime, IPackageEvent> _events = new();

    public IImmutableList<IPackageEvent> Events => _events.Values.ToImmutableList();

    public void AddEvent<T>(T packageEvent) where T : IPackageEvent
    {
        var stateMachine = new PackageStateMachine(this, SetStatus);

        switch (packageEvent)
        {
            case PackageDelivered:
                stateMachine.Deliver();
                break;
            case PackageRejected:
                stateMachine.Reject();
                break;
        }

        _events.Add(packageEvent.OccurredAt, packageEvent);
    }

    private void SetStatus(PackageStatus status)
    {
        Status = status;
    }
}

file class PackageStateMachine
{
    private readonly StateMachine<PackageStatus, PackageTrigger> _stateMachine;

    public PackageStateMachine(Package package, Action<PackageStatus> setStatus)
    {
        _stateMachine = new StateMachine<PackageStatus, PackageTrigger>(() => package.Status, setStatus);

        _stateMachine.Configure(PackageStatus.Pending)
            .Permit(PackageTrigger.Deliver, PackageStatus.Delivered)
            .Permit(PackageTrigger.Reject, PackageStatus.Rejected);
    }

    public void Deliver()
    {
        _stateMachine.Fire(PackageTrigger.Deliver);
    }

    public void Reject()
    {
        _stateMachine.Fire(PackageTrigger.Reject);
    }
}

file enum PackageTrigger
{
    Deliver,
    Reject
}