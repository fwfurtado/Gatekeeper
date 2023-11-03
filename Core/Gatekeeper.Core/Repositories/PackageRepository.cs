using Dapper;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.ValueObjects;
using Gatekeeper.Shared.Database;
using MediatR;

namespace Gatekeeper.Core.Repositories;

public class PackageRepository : IPackageRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public PackageRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExistsDescriptionAsync(string description, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM packages WHERE description = @description);";

        using var dbConnection = _connectionFactory.CreateConnection();

        var exists = await dbConnection.QuerySingleAsync<bool>(sql, new { description });

        return exists;
    }

    public async Task<PagedList<Package>> GetAll(PageRequest pageRequest, CancellationToken cancellationToken)
    {
        const string sql = "SELECT id, description, arrived_at, delivered_at, status FROM packages ORDER BY id LIMIT @size OFFSET @page;";

        using var dbConnection = _connectionFactory.CreateConnection();

        var packages = await dbConnection.QueryAsync<Package>(sql, new
        {
            size = pageRequest.Size,
            page = pageRequest.Page
        });
        var total = await CountAsync(cancellationToken);

        return new PagedList<Package>
        {
            Total = total,
            Data = packages.ToList()
        };
    }

    public async Task<Package?> GetByIdAsync(long packageId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT id, description, arrived_at, delivered_at, status FROM packages WHERE id = @packageId;";

        using var dbConnection = _connectionFactory.CreateConnection();

        var package = await dbConnection.QuerySingleOrDefaultAsync<Package?>(sql, new { packageId });

        return package;
    }

    public async Task<long> SaveAsync(Package package, CancellationToken cancellationToken)
    {
        const string sqlCommand = "INSERT INTO packages (description, delivered_at, arrived_at, status) " +
            "VALUES (@description, @delivered_at, @arrived_at, @status) RETURNING id;";

        using var dbConnection = _connectionFactory.CreateConnection();

        var arguments = new
        {
            description = package.Description,
            delivered_at = package.DeliveredAt,
            arrived_at = package.ArrivedAt,
            status = Enum.GetName(package.Status)
        };

        var id = await dbConnection.ExecuteScalarAsync<long>(sqlCommand, arguments);

        return id;
    }

    public async Task UpdateStatus(long packageId, PackageStatus status, CancellationToken cancellationToken)
    {
        
        using var dbConnection = _connectionFactory.CreateConnection();
        
        if(status != PackageStatus.Rejected)
        {
            const string sqlCommand = "UPDATE packages SET delivered_at = @delivered_at, status = @status WHERE id = @packageId;";
            var arguments = new
            {
                delivered_at = DateTime.UtcNow,
                status = Enum.GetName(status),
                packageId
            };
            await dbConnection.ExecuteScalarAsync<long>(sqlCommand, arguments);
        }
        else
        {
            const string sqlCommand = "UPDATE packages SET status = @status WHERE id = @packageId;";
            var arguments = new
            {
                status = Enum.GetName(status),
                packageId
            };
            await dbConnection.ExecuteScalarAsync<long>(sqlCommand, arguments);
        }

        

    }

    public async Task DeleteByIdAsync(long packageId, CancellationToken cancellationToken)
    {
        const string sql = "DELETE FROM packages WHERE id = @packageId;";

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, new { packageId });
    }

    private async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        const string sql = "SELECT COUNT(*) FROM packages;";

        using var dbConnection = _connectionFactory.CreateConnection();

        var count = await dbConnection.ExecuteScalarAsync<int>(sql);

        return count;
    }
}
