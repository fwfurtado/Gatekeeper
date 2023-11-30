namespace Gatekeeper.Rest.Domain.Package;

public interface IPackageEvent
{
    long PackageId { get; }
    DateTime OccurredAt { get; }
}

public record PackageRejected(long PackageId, string Reason) : IPackageEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

public record PackageDelivered(long PackageId) : IPackageEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}