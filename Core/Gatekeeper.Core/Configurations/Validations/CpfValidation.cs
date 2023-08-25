using FluentValidation;
using FluentValidation.Validators;
using Gatekeeper.Core.Policies;

namespace Gatekeeper.Core.Configurations.Validations;

public class CpfValidation<T> : PropertyValidator<T, string>
{
    private readonly IPolicy<string> _policy;

    public override string Name => "CpfValidation";

    protected override string GetDefaultMessageTemplate(string errorCode) => "{PropertyName} is not a valid CPF";

    public CpfValidation(IPolicy<string> policy)
    {
        _policy = policy;
    }

    public override bool IsValid(ValidationContext<T> context, string value)
    {
        return _policy.IsValid(value);
    }
}