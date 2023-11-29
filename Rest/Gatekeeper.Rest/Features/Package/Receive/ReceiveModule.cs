using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gatekeeper.Rest.Features.Package.Receive;

public record ReceivePackageRequest(long UnitId, string Description) : IRequest<long>;

public record ReceivePackageResponse(long Id);

public class ReceiveModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/packages", RequestHandler)
            .WithTags("Packages");
    }


    private static async Task<ReceivePackageResponse> RequestHandler(
        ISender sender,
        ILogger<ReceiveModule> logger,
        CancellationToken cancellationToken,
        [FromBody] ReceivePackageRequest request
    )
    {
        logger.LogInformation("Register a new package with params {Params}", request);

        var command = request.Adapt<ReceivePackageCommand>();

        var packageId = await sender.Send(command, cancellationToken);

        logger.LogInformation("Package registered with success");

        return new ReceivePackageResponse(packageId);
    }
}