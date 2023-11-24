using FluentAssertions;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Test.Fakers;
using static FluentAssertions.FluentActions;
namespace Gatekeeper.Core.Test.Entities;

public class PackageTest
{
    [Test]
    public void ShouldThrowAnErrorWhenDeliveringPackageAlreadyDelivered()
    {
        var package = new Package(1, "description", DateTime.Now, DateTime.Now, "Delivered", 1);
        Invoking(() => package.Deliver()).Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void ShouldThrowAnErrorWhenRejectingPackageAlreadyRejected()
    {
        var package = new Package(1, "description", DateTime.Now, DateTime.Now, "Rejected", 1);
        Invoking(() => package.Reject()).Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void ShouldThrowAnErrorWhenRejectingPackageAlreadyDelivered()
    {
        var package = new Package(1, "description", DateTime.Now, DateTime.Now, "Delivered", 1);
        Invoking(() => package.Reject()).Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void ShouldThrowAnErrorWhenDeliveringPackageAlreadyRejected()
    {
        var package = new Package(1, "description", DateTime.Now, DateTime.Now, "Rejected", 1);
        Invoking(() => package.Deliver()).Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void ShouldBeOk_WhenDelivering_APendingPackage()
    {
        var package = new Package("description", 1);
        package.Deliver();
        package.Status.Should().Be(PackageStatus.Delivered);
    }

    [Test]
    public void ShouldBeOk_WhenRejecting_APendingPackage()
    {
        var package = new Package("description", 1);
        package.Reject();
        package.Status.Should().Be(PackageStatus.Rejected);
    }
}