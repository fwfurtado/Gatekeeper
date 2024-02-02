namespace Gatekeeper.Rest;

public class NotificationSettings
{
    public const string SectionName = "Notification";

    public string TopicName { get; set; }
    public string TableName { get; set; }
    public PushNotificationSettings Push { get; set; }
}

public class PushNotificationSettings
{
    public string QueueName { get; set; }
}
