using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Gatekeeper.Rest.Configuration;
using Gatekeeper.Rest.DataLayer;
using Gatekeeper.Rest.Domain.Notification;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Gatekeeper.Rest.Test.DataLayer;

public class SendNotificationRepositoryTest : IDisposable
{
    private SendNotificationRepository _repository = null!;
    private AmazonDynamoDBClient _dynamoDbClient = null!;

    [SetUp]
    public void Setup()
    {
        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = "http://localhost:4566",
            AuthenticationRegion = "us-east-1"
        };

        var credentials = new BasicAWSCredentials("test", "test");

        _dynamoDbClient = new AmazonDynamoDBClient(credentials, config);

        var jsonOptions = new JsonOptions()
        {
            SerializerOptions =
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter() }
            }
        };
        var serializer = new DefaultJsonSerializer(Options.Create(jsonOptions));

        _repository = new SendNotificationRepository(_dynamoDbClient, serializer);
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
                { "message", "Hello World" },
                { "timestamp", DateTime.UtcNow }
            }
        };

        await _repository.SendAsync(notification, CancellationToken.None);

        var request = new GetItemRequest
        {
            TableName = "notifications",
            Key = new Dictionary<string, AttributeValue>
            {
                { "id", new AttributeValue { N = notification.Id.ToString() } }
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
