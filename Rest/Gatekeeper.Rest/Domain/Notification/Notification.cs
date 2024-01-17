namespace Gatekeeper.Rest.Domain.Notification;

public class Notification
{
    public required long Id { get; init; }

    public NotificationType Type { get; set; }

    public Dictionary<string, object> Payload { get; set; } = new();

    public DateTime CreatedAt { get; set; }
}
