using FluentValidation;
using FluentValidation.Validators;
using Gatekeeper.Core.Policies;

namespace Gatekeeper.Core.Configurations.Validations;

public class CpfValidation<T> : PropertyValidator<T, string>
{
    private readonly ISpecification<string> _specification;

    public override string Name => "CpfValidation";

    protected override string GetDefaultMessageTemplate(string errorCode) => "{PropertyName} is not a valid CPF";

    public CpfValidation(ISpecification<string> specification)
    {
        _specification = specification;
    }

    public override bool IsValid(ValidationContext<T> context, string value)
    {
        return _specification.IsValid(value);
    }
}