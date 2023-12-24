namespace Gatekeeper.Rest.Domain.Notification;

public class Notification
{
    public NotificationId Id { get; set; }

    public NotificationType Type { get; set; }

    public Dictionary<string, object> Payload { get; set; } = new();

    public DateTime CreatedAt { get; set; }
}