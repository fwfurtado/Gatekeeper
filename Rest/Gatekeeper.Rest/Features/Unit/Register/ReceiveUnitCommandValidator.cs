using FluentValidation;

namespace Gatekeeper.Rest.Features.Unit.Register;

public class ReceiveUnitCommandValidator : AbstractValidator<ReceiveUnitCommand>
{
    public ReceiveUnitCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty()
            .MaximumLength(255);
    }
}
