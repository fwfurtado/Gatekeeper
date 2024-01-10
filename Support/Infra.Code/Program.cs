using Pulumi;
using Pulumi.Aws.DynamoDB;
using System.Collections.Generic;
using Pulumi.Aws.DynamoDB.Inputs;

return await Deployment.RunAsync(() =>
{

    const string tableName = "notifications";
    var notifications = new Table(tableName, new TableArgs
    {
        Name = tableName,
        HashKey = "id",
        Attributes =
        {
            new TableAttributeArgs
            {
                Name = "id",
                Type = "N",
            }
        },
        BillingMode = "PAY_PER_REQUEST",
    });

    // Export the name of the bucket
    return new Dictionary<string, object?>
    {
        ["tableName"] = notifications.Name,
        ["primaryKey"] = notifications.HashKey,
    };
});