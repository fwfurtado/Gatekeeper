using Gatekeeper.Rest.Domain.Aggregate;
using MediatR;

namespace Gatekeeper.Rest.Domain.Package;

public interface IPackageEvent : INotification
{
    long PackageId { get; }
    DateTime OccurredAt { get; }
}

public record PackageRejected(long PackageId, DateTime OccurredAt, PackageStatus From, string Reason) : IPackageEvent
{
    public PackageStatus To => PackageStatus.Rejected;
}

public record PackageDelivered(long PackageId, DateTime OccurredAt, PackageStatus From) : IPackageEvent
{
    public PackageStatus To => PackageStatus.Delivered;
}

public record PackageReceived(long PackageId, long UnitId, string Description) : IPackageEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}