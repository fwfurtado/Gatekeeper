using Gatekeeper.Rest.Domain.Package;
using MediatR;

namespace Gatekeeper.Rest.EventHandlers;

public class PackageEventHandler<T>(
    IPackageEventSaver packageEventSaver
) : INotificationHandler<T> where T : IPackageEvent
{
    public Task Handle(T @event, CancellationToken cancellationToken)
    {
        return packageEventSaver.SaveAsync(@event, cancellationToken);
    }
}

public interface IPackageEventSaver
{
    Task SaveAsync<T>(T @event, CancellationToken cancellationToken) where T : IPackageEvent;
}