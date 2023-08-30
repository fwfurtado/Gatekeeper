using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Repositories;

namespace Gatekeeper.Rest;

public class MemoryUnitRepository : IUnitRepository
{
    private static long _identity = 1;
    private static readonly Dictionary<long, Unit> Database = new();
    
    public Task SaveAsync(Unit unit, CancellationToken cancellationToken)
    {
        Database.Add(_identity, unit);
        _identity++;
        
        return Task.CompletedTask;
    }

    public Task<bool> ExistsIdentifierAsync(string identifier, CancellationToken cancellationToken)
    {
        var result = Database.Any( u => u.Value.Identifier == identifier);

        return Task.FromResult(result);
    }

    public Task UpdateAsync(Unit unit, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Unit?> GetByIdAsync(long unitId, CancellationToken cancellationToken)
    {
        
        var found =Database.TryGetValue(unitId, out var unit);
        
        if (!found)
        {
            return Task.FromResult<Unit?>(null);
        }
        
        return Task.FromResult(unit);
    }
}