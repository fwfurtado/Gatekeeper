using System.Collections.Immutable;
using Gatekeeper.Rest.Domain.Aggregate;
using Stateless;

namespace Gatekeeper.Rest.Domain.Package;

public class Package
{
    public long Id { get; private set; }
    public string Description { get; set; }
    public long UnitId { get; set; }

    public DateTime ArrivedAt { get; } = DateTime.UtcNow;

    public PackageStatus Status { get; private set; } = PackageStatus.Pending;


    private readonly SortedList<DateTime, IPackageEvent> _events = new();

    public IImmutableList<IPackageEvent> Events => _events.Values.ToImmutableList();


    public static Package Factory(string description, long unitId)
    {
        var package = new Package
        {
            Description = description,
            UnitId = unitId
        };

        return package;
    }

    internal Package()
    {
    }

    public Package(IEnumerable<IPackageEvent> events) : this(events.ToArray())
    {
    }

    public Package(params IPackageEvent[] events) : this()
    {
        foreach (var @event in events)
        {
            AppendEvent(@event);
        }

        foreach (var (_, @event) in _events)
        {
            HandleEvent(@event);
        }
    }

    public void AddEvent<T>(T packageEvent) where T : IPackageEvent
    {
        AppendEvent(packageEvent);
        HandleEvent(packageEvent);
    }

    private void AppendEvent<T>(T packageEvent) where T : IPackageEvent
    {
        _events.Add(packageEvent.OccurredAt, packageEvent);
    }

    private void HandleEvent(IPackageEvent packageEvent)
    {
        switch (packageEvent)
        {
            case PackageReceived packageReceived:
                Apply(packageReceived);
                break;
            case PackageDelivered packageDelivered:
                Apply(packageDelivered);
                break;
            case PackageRejected packageRejected:
                Apply(packageRejected);
                break;
        }
    }

    private void Apply(PackageRejected packageRejected)
    {
        Status = packageRejected.To;
    }

    private void Apply(PackageDelivered packageDelivered)
    {
        Status = packageDelivered.To;
    }

    private void Apply(PackageReceived packageReceived)
    {
        Description = packageReceived.Description;
        UnitId = packageReceived.UnitId;
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