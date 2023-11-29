using Carter;
using MediatR;

namespace Gatekeeper.Rest.Features.Package.Reject;

public record RejectPackageRequest(string Reason);

public class PackageRejectModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/package/{id:long}/reject", RequestHandle)
            .WithTags("Packages");
    }

    private static async Task<IResult> RequestHandle(
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
}