using FluentValidation;
using Gatekeeper.Frontend.Admin.Dtos;

namespace Gatekeeper.Frontend.Admin.Validations;

public class UnitFormValidator : AbstractValidator<UnitForm>
{
    public UnitFormValidator()
    {
        RuleFor(f => f.Identifier)
            .NotEmpty();
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<UnitForm>.CreateWithOptions((UnitForm)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}