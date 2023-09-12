namespace Gatekeeper.Core.Policies;

public interface ISpecification<in T>
{
    bool IsValid(T value);
}