using Dapper;
using Gatekeeper.Shared.Database;

namespace Gatekeeper.Rest.Features.Package.Receive;

public interface IReceiveRepository
{
    Task<bool> ExistsDescriptionAsync(string description, CancellationToken cancellationToken);
    Task<long> SaveAsync(ReceivedPackage package, CancellationToken cancellationToken);
}

public class ReceiveRepository(IDbConnectionFactory connectionFactory) : IReceiveRepository
{
    public async Task<bool> ExistsDescriptionAsync(string description, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM packages WHERE description = @description);";

        using var dbConnection = connectionFactory.CreateConnection();

        var exists = await dbConnection.QuerySingleAsync<bool>(sql, new { description });

        return exists;
    }

    public async Task<long> SaveAsync(ReceivedPackage package, CancellationToken cancellationToken)
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
            Status = Enum.GetName(ReceivedPackage.Status),
            package.UnitId
        };

        var sqlCommand = new CommandDefinition(sql, arguments, cancellationToken: cancellationToken);

        var id = await dbConnection.ExecuteScalarAsync<long>(sqlCommand);

        return id;
    }
}