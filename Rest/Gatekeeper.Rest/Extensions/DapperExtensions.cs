using System.Data;
using Dapper;

namespace Gatekeeper.Rest.Extensions;

public static class DapperExtensions
{
    public static Task<T?> ExecuteScalarAsync<T>(
        this IDbConnection cnn,
        string sql,
        object param,
        CancellationToken cancellationToken,
        IDbTransaction? transaction = null
    )
    {

        var sqlCommand = new CommandDefinition(sql, param, cancellationToken: cancellationToken, transaction: transaction);
        
        return cnn.ExecuteScalarAsync<T>(sqlCommand);
    }
    
    public static Task<T?> ExecuteScalarAsync<T>(
        this IDbConnection cnn,
        string sql,
        CancellationToken cancellationToken,
        IDbTransaction? transaction = null
    )
    {

        var sqlCommand = new CommandDefinition(sql, cancellationToken: cancellationToken, transaction: transaction);
        
        return cnn.ExecuteScalarAsync<T>(sqlCommand);
    }
    
    public static Task<IEnumerable<T>> QueryAsync<T>(
        this IDbConnection cnn,
        string sql,
        object param,
        CancellationToken cancellationToken,
        IDbTransaction? transaction = null
    )
    {

        var sqlCommand = new CommandDefinition(sql, param, cancellationToken: cancellationToken, transaction: transaction);
        
        return cnn.QueryAsync<T>(sqlCommand);
    }
    
    public static Task<T?> QuerySingleOrDefaultAsync<T>(
        this IDbConnection cnn,
        string sql,
        object param,
        CancellationToken cancellationToken,
        IDbTransaction? transaction = null
    )
    {

        var sqlCommand = new CommandDefinition(sql, param, cancellationToken: cancellationToken, transaction: transaction);
        
        return cnn.QuerySingleOrDefaultAsync<T>(sqlCommand);
    }
    
    public static Task<int> ExecuteAsync(
        this IDbConnection cnn,
        string sql,
        object param,
        CancellationToken cancellationToken,
        IDbTransaction? transaction = null
    )
    {

        var sqlCommand = new CommandDefinition(sql, param, cancellationToken: cancellationToken, transaction: transaction);
        
        return cnn.ExecuteAsync(sqlCommand);
    }
}