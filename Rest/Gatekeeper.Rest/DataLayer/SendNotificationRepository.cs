using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Gatekeeper.Rest.Configuration;
using Gatekeeper.Rest.Domain.Notification;

namespace Gatekeeper.Rest.DataLayer;

public class SendNotificationRepository(IAmazonDynamoDB dynamoDbClient, IJsonSerializer jsonSerializer)

{
    private const string TableName = "notifications";

    public async Task<long> SendAsync(Notification notification, CancellationToken cancellationToken)
    {

        var item = jsonSerializer.Serialize(notification);


        var document = Document.FromJson(item);

        var table = Table.LoadTable(dynamoDbClient, "notifications");


        await table.PutItemAsync(document, cancellationToken);

        return 1;
    }
}
