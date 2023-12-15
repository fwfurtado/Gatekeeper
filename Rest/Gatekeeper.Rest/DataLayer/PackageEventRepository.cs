using System.Data;
using System.Text.Json;
using Dapper;
using Gatekeeper.Rest.Configuration;
using Gatekeeper.Rest.Domain.Package;
using Gatekeeper.Rest.EventHandlers;
using Gatekeeper.Rest.Extensions;
using Gatekeeper.Shared.Database;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Gatekeeper.Rest.DataLayer;

public class PackageEventRepository(
    IDbConnectionFactory connectionFactory,
    IJsonSerializer jsonSerializer
)
    : IPackageEventSaver
{
    public async Task SaveAsync<T>(T @event, CancellationToken cancellationToken) where T : IPackageEvent
    {
        const string sql = """
                           INSERT INTO package_events_history (package_id, event_type ,occured_at, metadata)
                            VALUES (@PackageId, @EventType, @OccurredAt, @Metadata);
                           """;

        var metadata = jsonSerializer.Serialize(@event);

        using var dbConnection = connectionFactory.CreateConnection();

        var arguments = new
        {
            EventType = @event.GetType().Name,
            @event.PackageId,
            @event.OccurredAt,
            Metadata = new JsonParameter(metadata)
        };

        await dbConnection.ExecuteAsync(sql, arguments, cancellationToken);
    }
}

file class JsonParameter(string value) : SqlMapper.ICustomQueryParameter
{
    public void AddParameter(IDbCommand command, string name)
    {
        var parameter = new NpgsqlParameter(name, NpgsqlDbType.Jsonb)
        {
            Value = value
        };

        command.Parameters.Add(parameter);
    }
}