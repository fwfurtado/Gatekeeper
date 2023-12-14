using System.Transactions;
using Gatekeeper.Core.ValueObjects;
using Gatekeeper.Rest.Domain.Package;
using Gatekeeper.Rest.Extensions;
using Gatekeeper.Rest.Features.Package.List;
using Gatekeeper.Rest.Features.Package.Receive;
using Gatekeeper.Rest.Features.Package.Reject;
using Gatekeeper.Rest.Features.Package.Remove;
using Gatekeeper.Rest.Features.Package.Show;
using Gatekeeper.Shared.Database;
using InvalidOperationException = System.InvalidOperationException;

namespace Gatekeeper.Rest.DataLayer;

public class PackageRepository(IDbConnectionFactory connectionFactory) :
    IPackageSaver,
    IPackageFetcherByDescription,
    IPackageListFetcher,
    IPackageFetcherById,
    IPackageRemover,
    IPackageSyncStatus
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

    public async Task<PagedList<Package>> FetchAsync(Pagination pagination, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT id Id, description Description, arrived_at ArrivedAt, status Status, target_unit_id UnitId
                           FROM packages
                           ORDER BY id
                           OFFSET @Offset
                           LIMIT @Limit;
                           """;
        using var conn = connectionFactory.CreateConnection();

        var arguments = new
        {
            Offset = pagination.Page,
            Limit = pagination.Size
        };

        var itemsInPage = await conn.QueryAsync<Package>(sql, arguments, cancellationToken);

        const string countSql = """
                                SELECT COUNT(*)
                                FROM packages;
                                """;


        var total = await conn.ExecuteScalarAsync<int>(countSql, cancellationToken: cancellationToken);

        return new PagedList<Package>
        {
            Total = total,
            Data = itemsInPage.ToList()
        };
    }

    public async Task<Package?> FetchAsync(long id, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT id Id, description Description, arrived_at ArrivedAt, status Status, target_unit_id UnitId
                           FROM packages
                           WHERE id = @id;
                           """;
        using var conn = connectionFactory.CreateConnection();

        return await conn.QuerySingleOrDefaultAsync<Package>(sql, new { id }, cancellationToken);
    }

    public async Task RemoveAsync(long id, CancellationToken cancellationToken)
    {
        const string sql = """
                           DELETE
                           FROM packages
                           WHERE id = @id;
                           """;

        using var conn = connectionFactory.CreateConnection();

        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var affect = await conn.ExecuteAsync(sql, new { id }, cancellationToken);

        if (affect != 1)
        {
            throw new InvalidOperationException($"Expected to remove 1 package, but removed {affect} packages.");
        }

        tx.Complete();
    }

    public async Task SyncStatus(Package package, CancellationToken cancellationToken)
    {
        const string sql = """
                           UPDATE packages
                           SET status = @status
                           WHERE id = @id;
                           """;


        using var conn = connectionFactory.CreateConnection();

        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var arguments = new
        {
            status = Enum.GetName(package.Status),
            id = package.Id
        };

        var affect = await conn.ExecuteAsync(sql, arguments, cancellationToken);


        if (affect != 1)
        {
            throw new InvalidOperationException($"Expected to update 1 package status, but updated {affect} packages.");
        }

        tx.Complete();
    }
}