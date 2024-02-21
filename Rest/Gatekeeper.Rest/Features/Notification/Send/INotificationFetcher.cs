namespace Gatekeeper.Rest.Features.Notification.Send;

public interface INotificationFetcher
{
    Task<Domain.Notification.Notification?> GetByIdAsync(long id, CancellationToken cancellationToken);
}
