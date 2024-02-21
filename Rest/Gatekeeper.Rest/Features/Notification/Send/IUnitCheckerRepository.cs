namespace Gatekeeper.Rest.Features.Notification.Send;

public interface IUnitCheckerRepository
{
    Task<bool> ExistsUnitById(long id, CancellationToken cancellationToken);
}
