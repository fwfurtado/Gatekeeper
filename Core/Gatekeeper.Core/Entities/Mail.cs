namespace Gatekeeper.Core.Entities;

public class Mail
{
    public Tenant Tenant { get; private set; }
    public string Description { get; private set; }
    public DateTime ArrivedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? DeliveredAt { get; private set; }

    public Mail(Tenant tenant, string description)
    {
        if (string.IsNullOrEmpty(description))
        {
            throw new ArgumentException("Description cannot be blank");
        }
        if (tenant is null)
        {
            throw new ArgumentException("Tenant is required");
        }

        Tenant = tenant;
        Description = description;
    }

    public void Delivered()
    {
        DeliveredAt = DateTime.UtcNow;
    }
}