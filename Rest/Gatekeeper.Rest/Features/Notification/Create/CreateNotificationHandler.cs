using FluentValidation;
using Gatekeeper.Rest.Domain.Notification;
using Mapster;
using MediatR;

namespace Gatekeeper.Rest.Features.Notification.Create;

public record CreateNotificationCommand(
    NotificationType Type,
    Dictionary<string, object> Payload
) : IRequest<Domain.Notification.Notification?>;

public class CreateNotificationHandler(
    IValidator<CreateNotificationCommand> validator,
    INotificationSaver notificationSaver
) : IRequestHandler<CreateNotificationCommand, Domain.Notification.Notification?>
{
    public async Task<Domain.Notification.Notification?> Handle(CreateNotificationCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        var saveCommand = request.Adapt<SaveNotificationCommand>();


        var notification = await notificationSaver.SaveAsync(saveCommand, cancellationToken);

        return notification;
    }
}
