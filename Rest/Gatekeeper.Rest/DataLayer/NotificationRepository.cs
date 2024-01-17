using Gatekeeper.Rest.Domain.Notification;
using Gatekeeper.Rest.Extensions;
using Gatekeeper.Rest.Features.Notification.Create;
using Gatekeeper.Shared.Database;

namespace Gatekeeper.Rest.DataLayer;

public class NotificationRepository(IDbConnectionFactory connectionFactory) : INotificationSaver
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
}
