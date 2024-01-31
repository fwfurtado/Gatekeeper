using Pulumi.Aws.DynamoDB;
using Pulumi.Aws.DynamoDB.Inputs;

namespace Infra.Code.Resources;

public class NotificationTable() : Table(TableName, Args)
{
    private const string TableName = "notifications";

    private static readonly TableArgs Args = new()
    {
        HashKey = "id",
        Attributes =
        {
            new TableAttributeArgs
            {
                Name = "id",
                Type = "N",
            }
        },
        Ttl = new TableTtlArgs
        {
          Enabled  = true,
          AttributeName = "ttl"
        },
        BillingMode = "PAY_PER_REQUEST",
    };
}
