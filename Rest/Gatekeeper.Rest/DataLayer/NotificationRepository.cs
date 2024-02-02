using System.Data;
using Dapper;
using Gatekeeper.Rest.Configuration;
using Gatekeeper.Rest.Domain.Notification;
using Gatekeeper.Rest.Extensions;
using Gatekeeper.Rest.Features.Notification.Create;
using Gatekeeper.Rest.Features.Notification.Send;
using Gatekeeper.Rest.Infra.Dapper;
using Gatekeeper.Shared.Database;
using Npgsql;
using NpgsqlTypes;

namespace Gatekeeper.Rest.DataLayer;

public class NotificationRepository(
    IDbConnectionFactory connectionFactory,
    IJsonSerializer jsonSerializer
) : INotificationSaver, INotificationFetcher
{
    public async Task<Notification> SaveAsync(SaveNotificationCommand command, CancellationToken cancellationToken)
    {
        using var conn = connectionFactory.CreateConnection();

        const string sql = """
                           INSERT INTO notifications (type, payload, created_at)
                            VALUES (@Type, @Payload, @CreatedAt) RETURNING id;
                           """;


        var jsonPayload = jsonSerializer.Serialize(command.Payload);

        var parameters = new
        {
            command.Type,
            Payload = new JsonParameter(jsonPayload),
            CreatedAt = DateTime.UtcNow
        };

        var id = await conn.ExecuteScalarAsync<long>(sql, parameters, cancellationToken);

        return new Notification
        {
            Id = id,
            Type = command.Type,
            Payload = command.Payload,
            CreatedAt = parameters.CreatedAt
        };
    }

    public async Task<Notification?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT id, type, payload, created_at FROM notifications WHERE id = @Id;
                           """;
        using var conn = connectionFactory.CreateConnection();

        var parameters = new { Id = id };
        var result = await conn.QuerySingleOrDefaultAsync<NotificationEntity>(sql, parameters, cancellationToken);

        if (result is null) return null;

        return new Notification
        {
            Id = id,
            Type = result.Type,
            Payload = result.Payload.Value,
            CreatedAt = result.CreatedAt
        };
    }
}

file class NotificationEntity
{
    public long Id { get; init; }
    public NotificationType Type { get; init; }
    public Json<Dictionary<string, object>> Payload { get; init; }
    public DateTime CreatedAt { get; init; }
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
