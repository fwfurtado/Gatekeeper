using FluentValidation;
using Gatekeeper.Shared.Validations.Brazilian;

namespace Gatekeeper.Frontend.Admin.Validations;

public class CpfValidator : AbstractValidator<string>
{
    private readonly CpfValidation _validation = new (); 
    
    public CpfValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("O CPF é obrigatório");

        RuleFor(x => x)
            .Must(x => x.All(char.IsDigit))
            .WithMessage("O CPF deve conter apenas números");

        RuleFor(x => x)
            .Must(x => x.Length == 11)
            .WithMessage("O CPF deve conter 11 dígitos");

        RuleFor(x => x)
            .Must(_validation.IsValid)
            .WithMessage("O CPF é inválido");
    }
    
    private IEnumerable<string> ValidateValue(string arg)
    {
        var result = Validate(arg);
        if (result.IsValid)
            return new string[0];
        return result.Errors.Select(e => e.ErrorMessage);
    }

    public Func<string, IEnumerable<string>> Validation => ValidateValue;
}