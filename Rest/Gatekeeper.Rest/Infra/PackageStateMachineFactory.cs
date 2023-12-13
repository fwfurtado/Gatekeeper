using Gatekeeper.Rest.Domain.Aggregate;
using Gatekeeper.Rest.Domain.Package;
using MediatR;

namespace Gatekeeper.Rest.Infra;

public class PackageStateMachineFactory(
    IPublisher publisher,
    IDateTimeProvider dateTimeProvider
) : IPackageStateMachineFactory
{
    
    public PackageStateMachine Factory(PackageStatus initial) 
    {
        return new PackageStateMachine(initial, publisher, dateTimeProvider);
    }
}

public interface IPackageStateMachineFactory
{
    PackageStateMachine Factory(PackageStatus initial);
}