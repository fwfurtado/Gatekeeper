using Gatekeeper.Rest.Domain.Package;
using Gatekeeper.Rest.Features.Package.Show;
using MediatR;

namespace Gatekeeper.Rest.Features.Package.Reject;

public record PackageRejectCommand(long PackageId, string Reason) : IRequest<PackageRejected?>;

public class PackageRejectRequestHandler(
    IPublisher publisher,
    IPackageFetcherById packageFetcherById,
    IPackageSyncStatus packageSyncStatus
) : IRequestHandler<PackageRejectCommand, PackageRejected?>
{
    public async Task<PackageRejected?> Handle(PackageRejectCommand command, CancellationToken cancellationToken)
    {
        var package = await packageFetcherById.FetchAsync(command.PackageId, cancellationToken);

        if (package is null)
        {
            return null;
        }

        var rejectEvent = new PackageRejected(command.PackageId, package.Status, command.Reason);

        package.AddEvent(rejectEvent);

        await packageSyncStatus.SyncStatus(package, cancellationToken);

        await publisher.Publish(rejectEvent, cancellationToken);

        return rejectEvent;
    }
}