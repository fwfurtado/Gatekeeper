
using MediatR;

namespace Gatekeeper.Rest.Features.Notification.Send;

public record SendNotificationCommand(long Id) : IRequest;

public class SendNotificationHandler(
    INotificationFetcher notificationFetcher
) : IRequestHandler<SendNotificationCommand>
{
    public async Task Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await notificationFetcher.GetByIdAsync(request.Id, cancellationToken);

        if (notification is null)
        {
            throw new ArgumentException("Notification not found", nameof(request));
        }


    }
}
