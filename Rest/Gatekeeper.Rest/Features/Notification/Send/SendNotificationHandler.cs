using FluentValidation;
using Gatekeeper.Rest.Consumers.PushNotification;
using Gatekeeper.Rest.Domain.Notification;
using MassTransit;
using MediatR;

namespace Gatekeeper.Rest.Features.Notification.Send;

public record SendNotificationCommand(long Id, long? UnitId = null) : IRequest;

public class SendNotificationHandler(
    INotificationFetcher notificationFetcher,
    IPublishEndpoint publisher,
    IUnitCheckerRepository unitCheckerRepository,
    IValidator<SendNotificationCommand> validator
    ) : IRequestHandler<SendNotificationCommand>
{
    public async Task Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
        var notification = await notificationFetcher.GetByIdAsync(request.Id, cancellationToken);

        if (notification is null)
        {
            throw new ArgumentException("Notification not found", nameof(request));
        }

        if (notification.Type == NotificationType.Global)
        {
            var unitSplitterEvent = new NotificationToUnitSplitter(
                Id: notification.Id!.Value
            );

            await publisher.Publish(unitSplitterEvent, cancellationToken);

            return;
        };


        if (request.UnitId is null) throw new ArgumentException("UnitId is required for global notifications", nameof(request));

        var existsUnit = await unitCheckerRepository.ExistsUnitById(request.UnitId.Value, cancellationToken);

        if (!existsUnit) throw new ArgumentException("Unit not found", nameof(request));

        var residentSplitterEvent = new NotificationToResidentSplitter(
            Id: notification.Id!.Value,
            UnitId: request.UnitId!.Value
        );

        await publisher.Publish(residentSplitterEvent, cancellationToken);
    }
}
