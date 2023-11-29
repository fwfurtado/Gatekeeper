using MediatR;

namespace Gatekeeper.Rest.Features.Package.Show;

public record PackageShowQuery(long Id) : IRequest<Domain.Package?>;

public class PackageShowRequestHandler(
    IPackageFetcherById fetcherById
    ): IRequestHandler<PackageShowQuery, Domain.Package?>
{
    public Task<Domain.Package?> Handle(PackageShowQuery query, CancellationToken cancellationToken)
    {
        return fetcherById.FetchAsync(query.Id, cancellationToken);
    }
}