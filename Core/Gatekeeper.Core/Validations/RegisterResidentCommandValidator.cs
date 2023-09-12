using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Specifications;

namespace Gatekeeper.Core.Validations;

public sealed class RegisterResidentCommandValidator: AbstractValidator<RegisterResidentCommand>
{ 
    
    public RegisterResidentCommandValidator(ICpfSpecification specification)
    {
        RuleFor(r => r.Name).Length(1, 100);
        RuleFor(r => r.Document).SetValidator(new CpfValidator(specification));
    }
}