using Amazon.DynamoDBv2;
using Amazon.Runtime;

namespace Gatekeeper.Rest.Infra.Aws;

public static class ClientExtensions
{
    public static IServiceCollection AddAws(this IServiceCollection service)
    {

        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = "http://localhost:4566",
            AuthenticationRegion = "us-east-1"
        };

        var credentials = new BasicAWSCredentials("test", "test");

        var dynamoDbClient = new AmazonDynamoDBClient(credentials, config);


        service.AddSingleton<IAmazonDynamoDB>(dynamoDbClient);

        return service;
    }
}
