using System.Collections.Generic;
using System.Text.Json;
using Pulumi.Aws.Sqs;

namespace Infra.Code.Resources;

public class NotificationQueueDlqFactory(NotificationQueue source)
{
    private readonly string _queueName =  $"{NotificationQueue.QueueName}-dlq";

    private readonly QueueArgs _args = new()
    {
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
        return new Queue(_queueName, _args);
    }
}
