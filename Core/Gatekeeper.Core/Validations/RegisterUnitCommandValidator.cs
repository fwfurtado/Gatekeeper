using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Policies;

namespace Gatekeeper.Core.Validations;

public class RegisterUnitCommandValidator : AbstractValidator<RegisterUnitCommand>
{
    public RegisterUnitCommandValidator(IUnitIdentifierDuplicatedPolicy policy)
    {
        RuleFor(command => command.Identifier)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(policy.IsValidAsync)
            .WithMessage("Identifier already exists");
    }
}