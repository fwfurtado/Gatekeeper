namespace Gatekeeper.Core.Entities;

public class Mail
{
    public Resident Resident { get; private set; }
    public string Description { get; private set; }
    public DateTime ArrivedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? DeliveredAt { get; private set; }

    public Mail(Resident resident, string description)
    {
        if (string.IsNullOrEmpty(description))
        {
            throw new ArgumentException("Description cannot be blank");
        }
        if (resident is null)
        {
            throw new ArgumentException("Resident is required");
        }

        Resident = resident;
        Description = description;
    }

    public void Delivered()
    {
        DeliveredAt = DateTime.UtcNow;
    }
}