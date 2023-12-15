using System.Transactions;
using Gatekeeper.Rest.Domain.Package;
using Gatekeeper.Rest.Features.Package.Show;
using Gatekeeper.Rest.Infra;
using MediatR;

namespace Gatekeeper.Rest.Features.Package.Reject;

public record PackageRejectCommand(long PackageId, string Reason) : IRequest<Domain.Package.Package?>;

public class PackageRejectRequestHandler(
    IPackageStateMachineFactory packageStateMachineFactory,
    IPackageFetcherById packageFetcherById,
    IPackageSyncStatus packageSyncStatus
) : IRequestHandler<PackageRejectCommand, Domain.Package.Package?>
{
    public async Task<Domain.Package.Package?> Handle(PackageRejectCommand command, CancellationToken cancellationToken)
    {
        var package = await packageFetcherById.FetchAsync(command.PackageId, cancellationToken);

        if (package is null)
        {
            return null;
        }

        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var stateMachine = packageStateMachineFactory.Factory(package.Status);

        await stateMachine.RejectAsync(package, command.Reason, cancellationToken);

        await packageSyncStatus.SyncStatus(package, cancellationToken);

        tx.Complete();

        return package;
    }
}