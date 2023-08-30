namespace Gatekeeper.Core.Policies;

public interface IAsyncPolicy<in T>
{
    Task<bool> IsValidAsync(T value, CancellationToken cancellationToken);
}