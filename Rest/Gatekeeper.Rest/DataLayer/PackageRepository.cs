using Gatekeeper.Rest.Domain;
using Gatekeeper.Rest.Extensions;
using Gatekeeper.Rest.Features.Package.Receive;
using Gatekeeper.Shared.Database;

namespace Gatekeeper.Rest.DataLayer;

public class PackageRepository(IDbConnectionFactory connectionFactory) : IReceiveRepository
{
    public async Task<bool> ExistsDescriptionAsync(string description, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM packages WHERE description = @description);";

        using var dbConnection = connectionFactory.CreateConnection();

        var exists = await dbConnection.ExecuteScalarAsync<bool>(sql, new { description }, cancellationToken);

        return exists;
    }

    public async Task<long> SaveAsync(Package package, CancellationToken cancellationToken)
    {
        const string sql = """
                                  INSERT INTO packages (description,arrived_at, status, target_unit_id)
                                  VALUES (@Description, @ArrivedAt, @Status, @UnitId) RETURNING id;
                                  """;

        using var dbConnection = connectionFactory.CreateConnection();

        var arguments = new
        {
            package.Description,
            package.ArrivedAt,
            Status = Enum.GetName(package.Status),
            package.UnitId
        };
        
        var id = await dbConnection.ExecuteScalarAsync<long>(sql, arguments, cancellationToken);

        return id;
    }
}