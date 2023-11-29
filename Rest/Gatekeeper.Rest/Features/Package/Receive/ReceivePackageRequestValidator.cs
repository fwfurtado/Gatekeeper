using FluentValidation;

namespace Gatekeeper.Rest.Features.Package.Receive;

public class ReceivePackageRequestValidator : AbstractValidator<ReceivePackageRequest>
{
    public ReceivePackageRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.UnitId).GreaterThan(0);
    }
}