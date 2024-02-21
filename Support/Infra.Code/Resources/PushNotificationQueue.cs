using Pulumi.Aws.Sns;
using Pulumi.Aws.Sqs;

namespace Infra.Code.Resources;

public class PushNotificationQueue() : Queue(QueueName, Args)
{
    public const string QueueName = "push-notification";

    private static readonly QueueArgs Args = new()
    {
        Name = QueueName
    };


    public TopicSubscription SubscribeTo(Topic topic)
    {
        return new TopicSubscription($"{topic.Name}-{Name}-subscription", new TopicSubscriptionArgs
        {
            Topic = topic.Arn,
            Protocol = "sqs",
            Endpoint = Arn,
        });
    }
}
