using Stateless;

namespace Gatekeeper.Core.Entities;

public class Package
{
    public long Id { get; set; }
    public string Description { get; private set; }
    public DateTime ArrivedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public PackageStatus Status { get; set; }
    public long UnitId { get; set; }

    private readonly PackageStateMachine _stateMachine;

    public Package(string description, long unit_id)
    {
        if (string.IsNullOrEmpty(description))
        {
            throw new ArgumentException("Description cannot be blank");
        }

        Description = description;
        ArrivedAt = DateTime.UtcNow;
        Status = PackageStatus.Pending;
        UnitId = unit_id;

        _stateMachine = new PackageStateMachine(this);
    }

    public Package(long id, string description, DateTime arrived_at, DateTime delivered_at, string status, long target_unit_id) : this(description, target_unit_id)
    {
        Id = id;
        ArrivedAt = arrived_at;
        DeliveredAt = delivered_at;
        Status = Enum.Parse<PackageStatus>(status);

        _stateMachine = new PackageStateMachine(this);
    }

    public void Deliver()
    {
        _stateMachine.Fire(PackageTriggers.Deliver);
    }

    public void Reject()
    {
        _stateMachine.Fire(PackageTriggers.Reject);
    }

}

public enum PackageTriggers
{
    Deliver,
    Reject,
}

public class PackageStateMachine : StateMachine<PackageStatus, PackageTriggers>
{
    public PackageStateMachine(Package package) : base(
        () => package.Status,
        newStatus => package.Status = newStatus
       )
    {
        Configure(PackageStatus.Pending)
            .Permit(PackageTriggers.Deliver, PackageStatus.Delivered)
            .Permit(PackageTriggers.Reject, PackageStatus.Rejected);
    }
}