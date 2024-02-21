using Gatekeeper.Rest.Consumers;
using Gatekeeper.Rest.Consumers.PushNotification;
using Gatekeeper.Rest.Domain.Notification;
using Gatekeeper.Rest.Test.Configurations;
using MassTransit.Testing;

namespace Gatekeeper.Rest.Test.Consumers;

[TestFixture]
public class NotificationConsumerTest : AcceptanceTest
{
    private ITestHarness _harness = null!;

    [SetUp]
    public void Setup()
    {
        _harness = Factory.Services.GetTestHarness();
    }


    [Test]
    public async Task ShouldReceiveNotification()
    {
        var sent = new NotificationSent(
            Id: 1,
            Type: NotificationType.Global,
            Payload: new Dictionary<string, object>
            {
                { "message", "Hello World" },
                { "timestamp", DateTime.UtcNow }
            },
            ReceiverId: 1
        );

        await _harness.Start();

        await _harness.Bus.Publish(sent);

        var consumer = _harness.GetConsumerHarness<NotificationConsumer>();

        Assert.That(await consumer.Consumed.Any<NotificationSent>(n => n.Context.Message.Id == sent.Id));
    }
}
