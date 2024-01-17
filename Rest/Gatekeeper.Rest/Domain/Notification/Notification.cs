namespace Gatekeeper.Rest.Domain.Notification;

public record Notification
{
    public long? Id { get; init; }

    public NotificationType Type { get; init; }

    public Dictionary<string, object> Payload { get; init; } = new();

    public DateTime CreatedAt { get; init; }
}
