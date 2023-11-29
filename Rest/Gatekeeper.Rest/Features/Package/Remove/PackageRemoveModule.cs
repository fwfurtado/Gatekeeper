using Carter;
using MediatR;

namespace Gatekeeper.Rest.Features.Package.Remove;

public class PackageRemoveModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/packages/{id:long}", RequestHandler)
            .WithTags("Packages");
    }

    private static async Task<IResult> RequestHandler(ISender sender, CancellationToken cancellationToken, long id)
    {
        var command = new PackageRemoveCommand(id);
        await sender.Send(command, cancellationToken);

        return Results.NoContent();
    }
}

