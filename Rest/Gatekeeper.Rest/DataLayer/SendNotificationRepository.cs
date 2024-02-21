using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Gatekeeper.Rest.Configuration;
using Gatekeeper.Rest.Consumers;
using Gatekeeper.Rest.Consumers.PushNotification;
using Gatekeeper.Rest.Domain.Notification;
using Gatekeeper.Rest.Infra;
using IdentityModel;

namespace Gatekeeper.Rest.DataLayer;

public class SendNotificationRepository(
    IDateTimeProvider dateTimeProvider,
    IAmazonDynamoDB dynamoDbClient,
    IJsonSerializer jsonSerializer,
    IConfiguration configuration
) : ISendNotificationRepository
{


    public async Task SendAsync(long userId, Notification notification, CancellationToken cancellationToken)
    {
        var notificationDocument = new NotificationDocument(
            Id: notification.Id!.Value,
            Type: notification.Type,
            Payload: notification.Payload,
            ReceiverId: userId,
            Ttl: dateTimeProvider.UtcNow.AddDays(3).ToEpochTime()
        );

        var item = jsonSerializer.Serialize(notificationDocument);

        var document = Document.FromJson(item);

        var settings = configuration.GetSection(NotificationSettings.SectionName).Get<NotificationSettings>()!;

        var table = Table.LoadTable(dynamoDbClient, settings.TableName);

        await table.PutItemAsync(document, cancellationToken);
    }

    private record NotificationDocument(
        long Id,
        NotificationType Type,
        Dictionary<string, object> Payload,
        long ReceiverId,
        long Ttl
    );
}
