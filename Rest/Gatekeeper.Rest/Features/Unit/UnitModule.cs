using Carter;
using Gatekeeper.Core.ValueObjects;
using Gatekeeper.Rest.Features.Unit.Filter;
using Gatekeeper.Rest.Features.Unit.List;
using Gatekeeper.Rest.Features.Unit.Register;
using Gatekeeper.Rest.Features.Unit.Show;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gatekeeper.Rest.Features.Unit;

public record RegisterUnitRequest(string Identifier);

public class UnitModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/units").WithTags("Units")
            .RequireAuthorization();

        group.MapPost("/", CreateHandler);
        group.MapGet("/", ListHandle);
        group.MapGet("/{id:long}", ShowHandle)
            .WithName("GetUnitById");
        group.MapGet("/filter", FilterHandle);
    }

    private static async Task<IResult> CreateHandler(
       ISender sender,
       ILogger<UnitModule> logger,
       CancellationToken cancellationToken,
       [FromBody] RegisterUnitRequest request
   )
    {
        logger.LogInformation("Register a new unit with params {Params}", request);

        var command = request.Adapt<ReceiveUnitCommand>();

        var unitId = await sender.Send(command, cancellationToken);

        logger.LogInformation("unit registered with success");

        return Results.CreatedAtRoute("GetUnitById", new { id = unitId }, null);
    }

    private static async Task<PagedList<Domain.Unit.Unit>> ListHandle(
        ISender sender,
        CancellationToken cancellationToken,
        [FromQuery] int? page,
        [FromQuery] int? size
    )
    {
        var pagination = new PageRequest(page ?? 0, size ?? 10);

        var query = pagination.Adapt<UnitListQuery>();
        return await sender.Send(query, cancellationToken);
    }

    private static async Task<IResult> ShowHandle(
        ISender sender,
        CancellationToken cancellationToken,
        [FromRoute] long id
    )
    {
        var query = new UnitShowQuery(id);
        var unit = await sender.Send(query, cancellationToken);

        return unit is not null ? Results.Ok(unit) : Results.NotFound();
    }

    private static async Task<IResult> FilterHandle(
        ISender sender,
        CancellationToken cancellationToken,
        [FromQuery] string? identifier = null
    )
    {
        var query = new UnitFilterQuery(identifier);
        var unit = await sender.Send(query, cancellationToken);

        return unit is not null ? Results.Ok(unit) : Results.NotFound();
    }
}
