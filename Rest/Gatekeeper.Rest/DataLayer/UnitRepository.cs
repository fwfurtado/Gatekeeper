using Gatekeeper.Core.ValueObjects;
using Gatekeeper.Rest.Domain;
using Gatekeeper.Rest.Extensions;
using Gatekeeper.Rest.Domain.Unit;
using Gatekeeper.Rest.Features.Unit.Filter;
using Gatekeeper.Rest.Features.Unit.List;
using Gatekeeper.Rest.Features.Unit.Register;
using Gatekeeper.Rest.Features.Unit.Show;
using Gatekeeper.Shared.Database;
using InvalidOperationException = System.InvalidOperationException;
using Dapper;

namespace Gatekeeper.Rest.DataLayer;

public class UnitRepository(IDbConnectionFactory connectionFactory) :
    IUnitSaver,
    IUnitFetcherById,
    IUnitListFetcher,
    IUnitFilter
{
    public async Task<long> SaveAsync(Unit unit, CancellationToken cancellationToken)
    {
        const string sqlCommand = """
                                  INSERT INTO units (identifier) 
                                  VALUES (@identifier) RETURNING id;
                                  """;

        using var dbConnection = connectionFactory.CreateConnection();

        var arguments = new
        {
            identifier = unit.Identifier
        };

        var id = await dbConnection.ExecuteScalarAsync<long>(sqlCommand, arguments);

        return id;
    }

    public async Task<bool> ExistsIdentifierAsync(string identifier, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT EXISTS(SELECT 1 
                           FROM units 
                           WHERE identifier = @identifier);
                           """;

        using var dbConnection = connectionFactory.CreateConnection();

        var exists = await dbConnection.ExecuteScalarAsync<bool>(sql, new { identifier }, cancellationToken);

        return exists;
    }

    public async Task<Unit?> GetByIdAsync(long unitId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT id, identifier FROM units WHERE id = @unitId;";

        using var dbConnection = connectionFactory.CreateConnection();

        var unit = await dbConnection.QuerySingleOrDefaultAsync<Unit?>(sql, new { unitId }, cancellationToken);

        return unit;
    }

    public async Task UpdateOccupationAsync(Unit unit, CancellationToken cancellationToken)
    {
        const string sql = "UPDATE units SET occupation_id = @occupationId WHERE id = @unitId;";

        using var connection = connectionFactory.CreateConnection();

        if (unit.Occupation is null)
        {
            throw new ArgumentException("Unit must have an occupation");
        }

        var arguments = new
        {
            occupationId = unit.Occupation.Id,
            unitId = unit.Id
        };

        await connection.ExecuteAsync(sql, arguments, cancellationToken);
    }

    public async Task<Unit?> GetByIdentifier(string identifier, CancellationToken cancellationToken)
    {
        const string sql = "SELECT id, identifier FROM units WHERE identifier = @identifier;";

        using var dbConnection = connectionFactory.CreateConnection();

        var unit = await dbConnection.QuerySingleOrDefaultAsync<Unit?>(sql, new { identifier }, cancellationToken);

        return unit;
    }

    public async Task<PagedList<Unit>> GetAll(PageRequest pageRequest, CancellationToken cancellationToken)
    {
        const string sql = "SELECT id, identifier FROM units ORDER BY id LIMIT @size OFFSET @page;";

        using var dbConnection = connectionFactory.CreateConnection();

        var units = await dbConnection.QueryAsync<Unit>(sql, new
        {
            size = pageRequest.Size,
            page = pageRequest.Page
        }, cancellationToken);
        var total = await CountAsync(cancellationToken);

        return new PagedList<Unit>
        {
            Total = total,
            Data = units.ToList()
        };
    }

    private async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        const string sql = "SELECT COUNT(*) FROM units;";

        using var dbConnection = connectionFactory.CreateConnection();

        var count = await dbConnection.ExecuteScalarAsync<int>(sql, cancellationToken);

        return count;
    }

    public async Task<IEnumerable<Unit?>> FilterByIdentifierAsync(string identifier, CancellationToken cancellationToken)
    {
        const string sql = "SELECT id, identifier FROM units WHERE identifier ILIKE @identifier;";

        using var dbConnection = connectionFactory.CreateConnection();

        var units = await dbConnection.QueryAsync<Unit>(sql, new { identifier = $"%{identifier}%" }, cancellationToken);

        return units;
    }

    public async Task<IEnumerable<Unit?>> GetTenFirstUnitsAsync(CancellationToken cancellationToken)
    {
        const string sql = "SELECT id, identifier FROM units ORDER BY id LIMIT 10;";

        using var dbConnection = connectionFactory.CreateConnection();

        var units = await dbConnection.QueryAsync<Unit>(sql, cancellationToken);

        return units;
    }

    public async Task<PagedList<Unit>> FetchAsync(Pagination pagination, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT id Id, identifier Identifier
                           FROM units
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

        var itemsInPage = await conn.QueryAsync<Unit>(sql, arguments, cancellationToken);

        const string countSql = """
                                SELECT COUNT(*)
                                FROM units;
                                """;

        var total = await conn.ExecuteScalarAsync<int>(countSql, cancellationToken: cancellationToken);

        return new PagedList<Unit>
        {
            Total = total,
            Data = itemsInPage.ToList()
        };
    }

    public async Task<Unit?> FetchAsync(long id, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT id Id, identifier Identifier
                           FROM units
                           WHERE id = @id;
                           """;
        using var conn = connectionFactory.CreateConnection();

        return await conn.QuerySingleOrDefaultAsync<Unit>(sql, new { id }, cancellationToken);
    }
}