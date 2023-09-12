using FluentValidation;
using Gatekeeper.Core.Specifications;
using Gatekeeper.Core.ValueObjects;

namespace Gatekeeper.Core.Validations;

public class CpfValidator: AbstractValidator<Cpf>
{
    public CpfValidator(ICpfSpecification specification)
    {
        RuleFor(c => c.Number)
            .NotEmpty()
            .Length(11)
            .Must(specification.IsValid)
            .WithMessage("Invalid CPF");
    }
}