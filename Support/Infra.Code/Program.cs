using System.Collections.Generic;
using Infra.Code.Resources;
using Pulumi;

return await Deployment.RunAsync(() =>
{
    var notificationsDynamoDbTable = new NotificationTable();
    var notificationsQueue = new NotificationQueue();
    var notificationsQueueDlq = new NotificationQueueDlqFactory(notificationsQueue).Factory();

    return new Dictionary<string, object?>
    {
        ["notification"] = new Dictionary<string, object>
        {
            ["dynamoDb"] = new Dictionary<string, object>
            {
                ["tableName"] = notificationsDynamoDbTable.Name,
                ["primaryKey"] = notificationsDynamoDbTable.HashKey,
            },
            ["queue"] = new Dictionary<string, object>
            {
                ["arn"] = notificationsQueue.Arn,
                ["name"] = notificationsQueue.Name,
                ["url"] = notificationsQueue.Url,

                ["dlq"] = new Dictionary<string, object>
                {
                    ["arn"] = notificationsQueueDlq.Arn,
                    ["name"] = notificationsQueueDlq.Name,
                    ["url"] = notificationsQueueDlq.Url,
                },
            },
        },
    };
});
