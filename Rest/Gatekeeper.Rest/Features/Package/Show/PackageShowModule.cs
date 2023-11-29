using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gatekeeper.Rest.Features.Package.Show;

public class PackageShowModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/packages/{id:long}", RequestHandler)
            .WithTags("Packages");
    }

    private static async Task<IResult> RequestHandler(
        ISender sender,
        CancellationToken cancellationToken,
        [FromRoute] long id
    )
    {
        var query = new PackageShowQuery(id);
        var package = await sender.Send(query, cancellationToken);

        return package is not null ? Results.Ok(package) : Results.NotFound();
    }
}