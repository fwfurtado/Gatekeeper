using Gatekeeper.Rest.Domain.Package;
using Gatekeeper.Rest.Features.Package.Reject;
using Gatekeeper.Rest.Features.Package.Show;
using Mapster;
using MediatR;

namespace Gatekeeper.Rest.Features.Package.Deliver;

public record PackageDeliverCommand(long PackageId) : IRequest<PackageDelivered?>;

public class PackageDeliverRequestHandler(
    IPackageFetcherById packageFetcherById,
    IPackageSyncStatus packageSyncStatus
) : IRequestHandler<PackageDeliverCommand, PackageDelivered?>
{
    public async Task<PackageDelivered?> Handle(PackageDeliverCommand command, CancellationToken cancellationToken)
    {
        var package = await packageFetcherById.FetchAsync(command.PackageId, cancellationToken);

        if (package is null)
        {
            return null;
        }

        var delivered = new PackageDelivered(command.PackageId);

        package.AddEvent(delivered);

        await packageSyncStatus.SyncStatus(package, cancellationToken);

        return delivered;
    }
}