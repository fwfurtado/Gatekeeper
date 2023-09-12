using FluentValidation;
using Gatekeeper.Core.Commands;

namespace Gatekeeper.Core.Validations;

public class RegisterUnitCommandValidator : AbstractValidator<RegisterUnitCommand>
{
    public RegisterUnitCommandValidator()
    {
        RuleFor(command => command.Identifier)
            .NotEmpty()
            .MaximumLength(100);
    }
}