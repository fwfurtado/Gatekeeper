using Gatekeeper.Shared.Validations.Brazilian;

namespace Gatekeeper.Core.Specifications;

public interface ICpfSpecification : ISpecification<string>
{
}

public class CpfSpecification : ICpfSpecification
{
    private readonly CpfValidation _cpfValidation = new();

    public bool IsValid(string value)
    {
        return _cpfValidation.IsValid(value);
    }
}