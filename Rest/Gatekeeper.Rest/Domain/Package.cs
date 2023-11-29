using Gatekeeper.Core.Entities;

namespace Gatekeeper.Rest.Domain;

public class Package
{
    public required string Description { get; init; }

    public DateTime ArrivedAt { get; } = DateTime.UtcNow;

    public required long UnitId { get; init; }

    public PackageStatus Status  => PackageStatus.Pending;
}