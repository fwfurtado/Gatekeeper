using System.Collections.Generic;
using Infra.Code.Resources;
using Pulumi;

return await Deployment.RunAsync(() =>
{
    var notificationsDynamoDbTable = new NotificationTable();
    var pushNotificationsQueue = new PushNotificationQueue();
    var pushNotificationsQueueDlq = new PushNotificationQueueDlqFactory(pushNotificationsQueue).Factory();

    var notificationTopic = new NotificationTopic();
    var subscription = pushNotificationsQueue.SubscribeTo(notificationTopic);


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
                ["arn"] = pushNotificationsQueue.Arn,
                ["name"] = pushNotificationsQueue.Name,
                ["url"] = pushNotificationsQueue.Url,

                ["dlq"] = new Dictionary<string, object>
                {
                    ["arn"] = pushNotificationsQueueDlq.Arn,
                    ["name"] = pushNotificationsQueueDlq.Name,
                    ["url"] = pushNotificationsQueueDlq.Url,
                },
            },
            ["topics"] = new Dictionary<string, object>
            {
                ["name"] = notificationTopic.Name,
                ["arn"] = notificationTopic.Arn,
                ["subscriptions"] = new Dictionary<string, object>
                {
                    ["topic"] = subscription.Topic,
                    ["protocol"] = subscription.Protocol,
                    ["endpoint"] = subscription.Endpoint,
                    ["arn"] = subscription.Arn,
                },
            }
        },
    };
});
