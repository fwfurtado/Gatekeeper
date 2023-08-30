namespace Gatekeeper.Core.Policies;

public interface IPolicy<in T>
{
    bool IsValid(T value);
}