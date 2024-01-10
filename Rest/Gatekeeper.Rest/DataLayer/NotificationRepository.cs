using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Gatekeeper.Rest.Domain.Notification;
using Gatekeeper.Rest.Features.Notification.Send;

namespace Gatekeeper.Rest.DataLayer;

public class NotificationRepository : INotificationSendRepository
{
    private const string TableName = "notifications";
    private readonly AmazonDynamoDBClient _dynamoDbClient;

    public NotificationRepository(AmazonDynamoDBClient dynamoDbClient)
    {
        _dynamoDbClient = dynamoDbClient;
    }

    public async Task SendAsync(Notification notification, CancellationToken cancellationToken)
    {


        var payload =
            notification.Payload.ToDictionary(k => k.Key, pair => new AttributeValue { S = Convert.ToString(pair.Value) });
        
        var request = new PutItemRequest
        {
            TableName=TableName,
            
            Item = new Dictionary<string, AttributeValue>
            {
                {"id", new AttributeValue {N = notification.Id.ToString()}},
                {"type", new AttributeValue {S = Enum.GetName(notification.Type)}},
                {"payload", new AttributeValue {M = payload}}
            }
        };
        
        
        var response = await _dynamoDbClient.PutItemAsync(request, cancellationToken);
        
        Console.WriteLine(response);
    }
}