using MassTransit;

namespace Gatekeeper.Rest.Consumers.PushNotification;


public record NotificationToUnitSplitter(
    long Id
);

public class UnitSplitterConsumer : IConsumer<NotificationToUnitSplitter>
{
    public Task Consume(ConsumeContext<NotificationToUnitSplitter> context)
    {
        throw new NotImplementedException();
    }
}
