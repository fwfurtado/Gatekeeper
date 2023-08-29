using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Policies;

namespace Gatekeeper.Core.Validations;

public sealed class RegisterResidentCommandValidator: AbstractValidator<RegisterResidentCommand>
{ 
    
    public RegisterResidentCommandValidator(ICpfPolicy policy)
    {
        RuleFor(r => r.Name).Length(1, 100);
        RuleFor(r => r.Document).SetValidator(new CpfValidator(policy));
    }
}