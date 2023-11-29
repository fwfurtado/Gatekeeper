using Carter;
using MediatR;

namespace Gatekeeper.Rest.Features.Package.Deliver;

public class PackageDeliverModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/package/{id:long}/deliver", RequestHandler)
            .WithTags("Packages");
    }
    
    
    private static async Task<IResult> RequestHandler(
        ISender sender,
        CancellationToken cancellationToken,
        long id
    )
    {
        var command = new PackageDeliverCommand(id);

        var delivered = await sender.Send(command, cancellationToken);

        return delivered is null ? Results.NotFound() : Results.Ok(delivered);
    }
}