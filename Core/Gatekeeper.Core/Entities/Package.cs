namespace Gatekeeper.Core.Entities;

public class Package
{
    public long Id { get; set; }
    public string Description { get; private set; }
    public DateTime ArrivedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public PackageStatus Status { get; set; }
    public long UnitId { get; set; }

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
    }

    public Package(long id, string description, DateTime arrived_at, DateTime delivered_at, string status, long target_unit_id) : this(description, target_unit_id)
    {
        Id = id;
        ArrivedAt = arrived_at;
        DeliveredAt = delivered_at;
        Status = Enum.Parse<PackageStatus>(status);
    }

}
