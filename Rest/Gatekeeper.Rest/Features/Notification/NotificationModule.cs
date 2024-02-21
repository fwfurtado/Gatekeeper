using Carter;
using Gatekeeper.Rest.Domain.Notification;
using Gatekeeper.Rest.Features.Notification.Create;
using Gatekeeper.Rest.Features.Notification.Send;
using Gatekeeper.Rest.Features.Package;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gatekeeper.Rest.Features.Notification;

public record CreateNotificationRequest(NotificationType Type, Dictionary<string, object> Payload);

public class NotificationModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/notifications").WithTags("Notifications");

        group.MapPost("/", CreateNotificationHandler);
        group.MapPut("/{id:long}", SendNotification);
    }

    private static async Task SendNotification(
        ISender sender,
        ILogger<PackageModule> logger,
        [FromRoute] long id,
        CancellationToken cancellationToken
    )
    {
        var command = new SendNotificationCommand(id);

        await sender.Send(command, cancellationToken);
    }

    private static async Task<IResult> CreateNotificationHandler(
        ISender sender,
        ILogger<PackageModule> logger,
        [FromBody] CreateNotificationRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Register a new package with params {Params}", request);

        var command = request.Adapt<CreateNotificationCommand>();

        var notification = await sender.Send(command, cancellationToken);

        if (notification is null)
        {
            return Results.BadRequest();
        }

        logger.LogInformation("Package registered with success");

        return Results.CreatedAtRoute("GetPackageById", new { id = notification.Id }, null);
    }
}
