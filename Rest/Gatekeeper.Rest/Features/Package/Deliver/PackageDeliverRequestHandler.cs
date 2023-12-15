using System.Transactions;
using Gatekeeper.Rest.Domain.Package;
using Gatekeeper.Rest.Features.Package.Reject;
using Gatekeeper.Rest.Features.Package.Show;
using Gatekeeper.Rest.Infra;
using MediatR;

namespace Gatekeeper.Rest.Features.Package.Deliver;

public record PackageDeliverCommand(long PackageId) : IRequest<Domain.Package.Package?>;

public class PackageDeliverRequestHandler(
    IPackageStateMachineFactory packageStateMachineFactory,
    IPackageFetcherById packageFetcherById,
    IPackageSyncStatus packageSyncStatus
) : IRequestHandler<PackageDeliverCommand, Domain.Package.Package?>
{
    public async Task<Domain.Package.Package?> Handle(PackageDeliverCommand command,
        CancellationToken cancellationToken)
    {
        var package = await packageFetcherById.FetchAsync(command.PackageId, cancellationToken);

        if (package is null)
        {
            return null;
        }

        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var stateMachine = packageStateMachineFactory.Factory(package.Status);

        await stateMachine.DeliverAsync(package, cancellationToken);

        await packageSyncStatus.SyncStatus(package, cancellationToken);
        
        tx.Complete();

        return package;
    }
}