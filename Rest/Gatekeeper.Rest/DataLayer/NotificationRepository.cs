using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Gatekeeper.Rest.Configuration;
using Gatekeeper.Rest.Domain.Notification;
using Gatekeeper.Rest.Features.Notification.Send;

namespace Gatekeeper.Rest.DataLayer;

public class NotificationRepository : INotificationSendRepository
{
    private const string TableName = "notifications";
    private readonly AmazonDynamoDBClient _dynamoDbClient;
    private readonly IJsonSerializer _jsonSerializer;

    public NotificationRepository(AmazonDynamoDBClient dynamoDbClient, IJsonSerializer jsonSerializer)
    {
        _dynamoDbClient = dynamoDbClient;
        _jsonSerializer = jsonSerializer;
    }

    public async Task SendAsync(Notification notification, CancellationToken cancellationToken)
    {


        var item = _jsonSerializer.Serialize(notification);


        var document = Document.FromJson(item);

        var table = Table.LoadTable(_dynamoDbClient, "notifications");


        await table.PutItemAsync(document, cancellationToken);
    }
}
