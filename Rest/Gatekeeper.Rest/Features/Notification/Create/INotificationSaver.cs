using Gatekeeper.Rest.Domain.Notification;

namespace Gatekeeper.Rest.Features.Notification.Create;

public record SaveNotificationCommand(
    NotificationType Type,
    Dictionary<string, object> Payload,
    DateTime? CreatedAt = null
);

public interface INotificationSaver
{
    Task<Domain.Notification.Notification> SaveAsync(SaveNotificationCommand command,
        CancellationToken cancellationToken);
}
