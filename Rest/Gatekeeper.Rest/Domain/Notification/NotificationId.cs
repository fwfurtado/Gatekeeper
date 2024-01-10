namespace Gatekeeper.Rest.Domain.Notification;

public readonly struct NotificationId(long Value)
{
    public static implicit operator NotificationId(long value) => new(value);
}