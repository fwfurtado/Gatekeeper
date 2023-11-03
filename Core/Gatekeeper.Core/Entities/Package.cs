namespace Gatekeeper.Core.Entities;

public class Package
{
    public long Id { get; set; }
    public string Description { get; private set; }
    public DateTime ArrivedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public PackageStatus Status { get; set; }

    public Package(string description)
    {
        if (string.IsNullOrEmpty(description))
        {
            throw new ArgumentException("Description cannot be blank");
        }

        Description = description;
        ArrivedAt = DateTime.UtcNow;
        Status = PackageStatus.Pending;
    }

    public Package(long id, string description, DateTime arrived_at, DateTime delivered_at, string status) : this(description)
    {
        Id = id;
        ArrivedAt = arrived_at;
        DeliveredAt = delivered_at;
        Status = Enum.Parse<PackageStatus>(status);
    }

}
