using Carter;
using Gatekeeper.Core.ValueObjects;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gatekeeper.Rest.Features.Package.List;

public class PackageListModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/packages", RequestHandler)
            .WithTags("Packages");
    }

    private static async Task<PagedList<Domain.Package>> RequestHandler(
        ISender sender,
        CancellationToken cancellationToken,
        [FromQuery] int? page,
        [FromQuery] int? size
    )
    {
        var pagination = new PageRequest(page ?? 0, size ?? 10);

        var query = pagination.Adapt<PackageListQuery>();
        return await sender.Send(query, cancellationToken);
    }
}