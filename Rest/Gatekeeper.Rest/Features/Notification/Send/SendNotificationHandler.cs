using Gatekeeper.Rest.Consumers;
using MassTransit;
using MediatR;

namespace Gatekeeper.Rest.Features.Notification.Send;

public record SendNotificationCommand(long Id) : IRequest;

public class SendNotificationHandler(
    INotificationFetcher notificationFetcher,
    IBus bus
) : IRequestHandler<SendNotificationCommand>
{
    public async Task Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await notificationFetcher.GetByIdAsync(request.Id, cancellationToken);

        if (notification is null)
        {
            throw new ArgumentException("Notification not found", nameof(request));
        }

        var sent = new NotificationSent(
            Id: notification.Id!.Value,
            Type: notification.Type,
            Payload: notification.Payload,
            ReceiverId: 1
        );

        await bus.Publish(sent, cancellationToken);
    }
}
