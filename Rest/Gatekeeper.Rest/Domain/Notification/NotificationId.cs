namespace Gatekeeper.Rest.Domain.Notification;

public readonly struct NotificationId(long value)
{
    public long Value { get; } = value;
    public static implicit operator NotificationId(long value) => new(value);
}
