using Gatekeeper.Rest.Features.Package.Show;
using MediatR;

namespace Gatekeeper.Rest.Features.Package.Remove;

public record PackageRemoveCommand(long Id) : IRequest<bool>;

public class PackageRemoveRequestHandler(
    IPackageFetcherById fetcherById,
    IPackageRemover remover
) : IRequestHandler<PackageRemoveCommand, bool>
{
    public async Task<bool> Handle(PackageRemoveCommand command, CancellationToken cancellationToken)
    {
        var id = command.Id;

        var package = await fetcherById.FetchAsync(id, cancellationToken);

        if (package is null) return false;

        await remover.RemoveAsync(id, cancellationToken);

        return true;
    }
}