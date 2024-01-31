using Amazon.DynamoDBv2;
using Amazon.Runtime;

namespace Gatekeeper.Rest.Infra.Aws;

public static class ClientExtensions
{

    private static readonly AWSCredentials LocalStackCredentials = new BasicAWSCredentials("test", "test");
    private static readonly AWSCredentials AwsCredentials = new EnvironmentVariablesAWSCredentials();

    private static readonly Action<dynamic> EnableLocalStackFor = config =>
    {
        config.ServiceURL = "http://localhost:4566";
        config.AuthenticationRegion = "us-east-1";
    };

    private static readonly Func<IHostEnvironment, AWSCredentials> GetCredentialsFor = environment => environment.IsDevelopment() ? LocalStackCredentials : AwsCredentials;


    public static IServiceCollection AddAws(this IServiceCollection service, IHostEnvironment environment)
    {

        var config = new AmazonDynamoDBConfig();

        if (environment.IsDevelopment())
        {
            EnableLocalStackFor(config);
        }

        var credentials = GetCredentialsFor(environment);

        var dynamoDbClient = new AmazonDynamoDBClient(credentials, config);


        service.AddSingleton<IAmazonDynamoDB>(dynamoDbClient);

        return service;
    }
}
