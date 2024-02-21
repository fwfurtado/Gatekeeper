using Pulumi.Aws.Sns;

namespace Infra.Code.Resources;

public class NotificationTopic() : Topic(TopicName, Args)
{
    private const string TopicName = "notifications";

    private static readonly TopicArgs Args = new()
    {
        Name = TopicName
    };
}
