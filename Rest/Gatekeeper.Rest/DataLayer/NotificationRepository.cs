using Gatekeeper.Rest.Domain.Notification;
using Gatekeeper.Rest.Extensions;
using Gatekeeper.Rest.Features.Notification.Create;
using Gatekeeper.Rest.Features.Notification.Send;
using Gatekeeper.Shared.Database;

namespace Gatekeeper.Rest.DataLayer;

public class NotificationRepository(IDbConnectionFactory connectionFactory) : INotificationSaver, INotificationFetcher
{
    public async Task<Notification> SaveAsync(SaveNotificationCommand command, CancellationToken cancellationToken)
    {
        using var conn = connectionFactory.CreateConnection();

        const string sql = """
                           INSERT INTO notifications (type, payload, created_at)
                            VALUES (@Type, @Payload, @CreatedAt) RETURNING id, type, payload, created_at;
                           """;

        var notification = await conn.QuerySingleAsync<Notification>(sql, command, cancellationToken);

        return notification;
    }

    public async Task<Notification?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT id, type, payload, created_at FROM notifications WHERE id = @Id;
                           """;
        using var conn = connectionFactory.CreateConnection();

        var parameters = new { Id = id };
        var notification = await conn.QuerySingleOrDefaultAsync<Notification>(sql, parameters, cancellationToken);

        return notification;
    }
}
