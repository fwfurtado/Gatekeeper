using MassTransit;

namespace Gatekeeper.Rest.Consumers.PushNotification;

public record NotificationToResidentSplitter(
    long Id,
    long UnitId
);

public class ResidentSplitterConsumer : IConsumer<NotificationToResidentSplitter>
{
    public Task Consume(ConsumeContext<NotificationToResidentSplitter> context)
    {
        throw new NotImplementedException();
    }
}
