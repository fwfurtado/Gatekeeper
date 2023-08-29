using FluentValidation;
using Gatekeeper.Core.Policies;
using Gatekeeper.Core.ValueObjects;

namespace Gatekeeper.Core.Validations;

public class CpfValidator: AbstractValidator<Cpf>
{
    public CpfValidator(ICpfPolicy policy)
    {
        RuleFor(c => c.Number)
            .NotEmpty()
            .Length(11)
            .Must(policy.IsValid)
            .WithMessage("Invalid CPF");
    }
}