using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Repositories;

public interface IOccupationRepository
{
    Task SaveOccupationAsync(Occupation occupation, CancellationToken cancellationToken);
}