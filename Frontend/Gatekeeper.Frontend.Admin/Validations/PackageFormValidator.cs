using FluentValidation;
using Gatekeeper.Frontend.Admin.Dtos;

namespace Gatekeeper.Frontend.Admin.Validations;

public class PackageFormValidator : AbstractValidator<PackageForm>
{
     
    
    public PackageFormValidator()
    {
        RuleFor(f => f.Description)
            .NotEmpty();

        RuleFor(f => f.UnitId)
            .NotEmpty();
    }
    
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<PackageForm>.CreateWithOptions((PackageForm)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}