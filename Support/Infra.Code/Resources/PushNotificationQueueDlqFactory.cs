using System.Collections.Generic;
using System.Text.Json;
using Pulumi.Aws.Sqs;

namespace Infra.Code.Resources;

public class PushNotificationQueueDlqFactory(Queue source)
{
    private const string QueueName = $"{PushNotificationQueue.QueueName}-dlq";

    private readonly QueueArgs _args = new()
    {
        Name = QueueName,
        RedriveAllowPolicy = JsonSerializer.Serialize(new Dictionary<string, object?>
        {
            ["redrivePermission"] = "byQueue",
            ["sourceQueueArns"] = new[]
            {
                source.Arn
            },
        }),
    };

    public Queue Factory()
    {
        return new Queue(QueueName, _args);
    }
}
