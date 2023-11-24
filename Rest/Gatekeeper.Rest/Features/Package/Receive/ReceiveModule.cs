using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gatekeeper.Rest.Features.Package.Receive;

public record ReceivePackageCommand(long UnitId, string Description) : IRequest<long>;

public class ReceiveModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/packages", async (
            ISender sender,
            ILogger<ReceiveModule> logger,
            CancellationToken cancellationToken,
            [FromBody] ReceivePackageCommand command
        ) =>
        {
            logger.LogInformation("Register a new package with params {Params}", command);

            var packageId = await sender.Send(command, cancellationToken);

            logger.LogInformation("Package registered with success");

            return  new { packageId };
        });
    }
}