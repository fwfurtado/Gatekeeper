using FluentValidation;

namespace Gatekeeper.Rest.Features.Package.Receive;

public class ReceivePackageCommandValidator : AbstractValidator<ReceivePackageCommand>
{
    public ReceivePackageCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.UnitId).GreaterThan(0);
    }
}