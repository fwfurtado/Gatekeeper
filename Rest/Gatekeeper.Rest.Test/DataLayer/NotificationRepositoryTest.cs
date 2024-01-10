using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Internal;
using Amazon.DynamoDBv2.Model;
using Gatekeeper.Rest.DataLayer;
using Gatekeeper.Rest.Domain.Notification;

namespace Gatekeeper.Rest.Test.DataLayer;

public class NotificationRepositoryTest : IDisposable
{
    private NotificationRepository _repository = null!;
    private AmazonDynamoDBClient _dynamoDbClient = null!;

    [SetUp]
    public void Setup()
    {
        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = "http://localhost:4566"
        };
        _dynamoDbClient = new AmazonDynamoDBClient(config);

        _repository = new NotificationRepository(_dynamoDbClient);
    }

    [Test]
    public async Task AddNotification()
    {
        var notification = new Notification
        {
            Id = 1,
            Type = NotificationType.Global,
            Payload = new Dictionary<string, object>
            {
                {"message", "Hello World"},
                {"timestamp", DateTime.UtcNow}
            }
        };
        
        await _repository.SendAsync(notification, CancellationToken.None);
        
        var request = new GetItemRequest
        {
            TableName = "notifications",
            Key = new Dictionary<string, AttributeValue>
            {
                {"id", new AttributeValue {N = notification.Id.ToString()}}
            }
        };
        
        var response = await _dynamoDbClient.GetItemAsync(request, CancellationToken.None);
        
        Assert.That(response, Is.Not.Null);
        
    }

    public void Dispose()
    {
        _dynamoDbClient.Dispose();
    }
}