namespace Gatekeeper.Core.Specifications;

public interface ISpecification<in T>
{
    bool IsValid(T value);
}