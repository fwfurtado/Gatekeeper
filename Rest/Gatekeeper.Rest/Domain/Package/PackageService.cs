using Gatekeeper.Rest.Domain.Aggregate;
using Stateless;

namespace Gatekeeper.Rest.Domain.Package;

public class PackageService
{
    public void Deliver(Package package)
    {
        var sateMachine = new PackageStateMachine(package);
        sateMachine.Deliver();

        var packageId = package.Id!.Value;

        package.AddEvent(new PackageDelivered(packageId));
    }

    public void Reject(Package package, string reason)
    {
        var sateMachine = new PackageStateMachine(package);
        sateMachine.Reject();

        var packageId = package.Id!.Value;

        package.AddEvent(new PackageRejected(packageId, reason));
    }
}

file class PackageStateMachine
{
    private readonly StateMachine<PackageStatus, PackageTrigger> _stateMachine;

    public PackageStateMachine(Package package)
    {
        _stateMachine = new StateMachine<PackageStatus, PackageTrigger>(package.Status);

        _stateMachine.Configure(PackageStatus.Pending)
            .Permit(PackageTrigger.Deliver, PackageStatus.Delivered)
            .Permit(PackageTrigger.Reject, PackageStatus.Rejected);
    }

    public void Deliver()
    {
        _stateMachine.Fire(PackageTrigger.Deliver);
    }

    public void Reject()
    {
        _stateMachine.Fire(PackageTrigger.Reject);
    }
}

file enum PackageTrigger
{
    Deliver,
    Reject
}