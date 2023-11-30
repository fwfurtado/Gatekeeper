using Carter;
using Gatekeeper.Core.ValueObjects;
using Gatekeeper.Rest.Features.Package.Deliver;
using Gatekeeper.Rest.Features.Package.List;
using Gatekeeper.Rest.Features.Package.Receive;
using Gatekeeper.Rest.Features.Package.Reject;
using Gatekeeper.Rest.Features.Package.Remove;
using Gatekeeper.Rest.Features.Package.Show;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gatekeeper.Rest.Features.Package;

public record ReceivePackageRequest(long UnitId, string Description);

public record RejectPackageRequest(string Reason);

public class PackageModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/packages").WithTags("Packages");


        group.MapPost("/", CreateHandler);
        group.MapGet("/", ListHandle);
        group.MapGet("/{id:long}", ShowHandle)
            .WithName("GetPackageById");

        group.MapPut("/{id:long}/reject", RejectHandle);
        group.MapPut("/{id:long}/deliver", DeliverHandle);
        group.MapDelete("/{id:long}", DeleteHandle);
    }


    private static async Task<IResult> CreateHandler(
        ISender sender,
        ILogger<PackageModule> logger,
        CancellationToken cancellationToken,
        [FromBody] ReceivePackageRequest request
    )
    {
        logger.LogInformation("Register a new package with params {Params}", request);

        var command = request.Adapt<ReceivePackageCommand>();

        var packageId = await sender.Send(command, cancellationToken);

        logger.LogInformation("Package registered with success");

        return Results.CreatedAtRoute("GetPackageById", new { id = packageId }, null);
    }

    private static async Task<PagedList<Domain.Package.Package>> ListHandle(
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

    private static async Task<IResult> ShowHandle(
        ISender sender,
        CancellationToken cancellationToken,
        [FromRoute] long id
    )
    {
        var query = new PackageShowQuery(id);
        var package = await sender.Send(query, cancellationToken);

        return package is not null ? Results.Ok(package) : Results.NotFound();
    }

    private static async Task<IResult> RejectHandle(
        ISender sender,
        CancellationToken cancellationToken,
        long id,
        RejectPackageRequest request
    )
    {
        var command = new PackageRejectCommand(id, request.Reason);

        var rejected = await sender.Send(command, cancellationToken);

        return rejected is null ? Results.NotFound() : Results.Ok(rejected);
    }


    private static async Task<IResult> DeliverHandle(
        ISender sender,
        CancellationToken cancellationToken,
        [FromRoute] long id
    )
    {
        var command = new PackageDeliverCommand(id);

        var delivered = await sender.Send(command, cancellationToken);

        return delivered is null ? Results.NotFound() : Results.Ok(delivered);
    }


    private static async Task<IResult> DeleteHandle(
        ISender sender,
        CancellationToken cancellationToken,
        [FromRoute] long id
    )
    {
        var command = new PackageRemoveCommand(id);
        await sender.Send(command, cancellationToken);

        return Results.NoContent();
    }
}