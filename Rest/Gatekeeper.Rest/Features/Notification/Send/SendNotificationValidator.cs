using FluentValidation;

namespace Gatekeeper.Rest.Features.Notification.Send;

public class SendNotificationValidator : AbstractValidator<SendNotificationCommand>
{
    public SendNotificationValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .GreaterThan(0);


        RuleFor(x => x.UnitId)
            .GreaterThan(0)
            .When(x => x.UnitId.HasValue);
    }
}
