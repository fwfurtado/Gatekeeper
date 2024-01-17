using Pulumi.Aws.Sqs;

namespace Infra.Code.Resources;

public class NotificationQueue() : Queue(QueueName, Args)
{
    public const string QueueName = "send-notifications";

    private static readonly QueueArgs Args = new();
}
