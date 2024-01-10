namespace Gatekeeper.Rest.Features.Notification.Send;

using Domain.Notification;

public interface INotificationSendRepository
{
    Task SendAsync(Notification notification, CancellationToken cancellationToken);
}