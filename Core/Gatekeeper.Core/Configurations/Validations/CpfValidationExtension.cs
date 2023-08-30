using FluentValidation;
using Gatekeeper.Core.Policies;

namespace Gatekeeper.Core.Configurations.Validations;

public static class CpfValidationExtension
{
    public static IRuleBuilder<T, string> Cpf<T>(this IRuleBuilder<T, string> ruleBuilder, ICpfPolicy service)
    {
        return ruleBuilder.SetValidator(new CpfValidation<T>(service));
    }
}