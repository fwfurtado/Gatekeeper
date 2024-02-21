using Gatekeeper.Rest.Domain.Notification;
using MassTransit;

namespace Gatekeeper.Rest.Consumers.PushNotification;

public record NotificationSent(
    long Id,
    NotificationType Type,
    Dictionary<string, object> Payload,
    long ReceiverId
);

public interface ISendNotificationRepository
{
    Task SendAsync(long userId, Notification notification, CancellationToken cancellationToken);
}

public class NotificationConsumer(
    ISendNotificationRepository repository,
    ILogger<NotificationConsumer> logger
) : IConsumer<NotificationSent>
{
    public async Task Consume(ConsumeContext<NotificationSent> context)
    {
        var notification = new Notification
        {
            Id = context.Message.Id,
            Type = context.Message.Type,
            Payload = context.Message.Payload,
            CreatedAt = DateTime.UtcNow
        };

        await repository.SendAsync(context.Message.ReceiverId, notification, context.CancellationToken);

        logger.LogInformation("Notification sent to user {UserId}", context.Message.ReceiverId);
    }
}
