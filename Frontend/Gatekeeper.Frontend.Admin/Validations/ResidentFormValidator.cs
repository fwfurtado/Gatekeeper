using FluentValidation;
using Gatekeeper.Frontend.Admin.Dtos;

namespace Gatekeeper.Frontend.Admin.Validations;

public class ResidentFormValidator : AbstractValidator<ResidentForm>
{
     
    
    public ResidentFormValidator(CpfValidator cpfValidator)
    {
        RuleFor(f => f.Name)
            .NotEmpty();

        RuleFor(f => f.Document)
            .NotEmpty()
            .SetValidator(cpfValidator);
    }
    
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<ResidentForm>.CreateWithOptions((ResidentForm)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}