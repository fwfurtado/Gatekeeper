using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Gatekeeper.Rest.Configuration;
using Gatekeeper.Rest.DataLayer;
using Gatekeeper.Rest.Domain.Notification;
using Gatekeeper.Rest.Infra;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Configuration;
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


        var inMemoryConfig = new Dictionary<string, string>
        {
            {"Notifications:TopicName", "notifications"},
            {"Notifications:TableName", "notifications"},
            {"Notifications:Push:QueueName", "push-notification"}
        };

       var configs = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemoryConfig!)
            .Build();

        var provider = new DateTimeProvider();
        _repository = new SendNotificationRepository(provider, _dynamoDbClient, serializer, configs);
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

        await _repository.SendAsync(1, notification, CancellationToken.None);

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
