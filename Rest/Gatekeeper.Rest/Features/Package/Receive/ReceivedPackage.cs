using Gatekeeper.Core.Entities;

namespace Gatekeeper.Rest.Features.Package.Receive;

public class ReceivedPackage
{
    public required string Description { get; init; }
    public DateTime ArrivedAt { get; } = DateTime.UtcNow;
    public required long UnitId { get; init; }

    public static PackageStatus Status => PackageStatus.Pending;
}