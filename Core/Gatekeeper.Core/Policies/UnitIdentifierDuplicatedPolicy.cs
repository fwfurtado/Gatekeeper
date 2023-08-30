using Gatekeeper.Core.Repositories;

namespace Gatekeeper.Core.Policies;

public interface IUnitIdentifierDuplicatedPolicy : IAsyncPolicy<string>
{
}

public class UnitIdentifierDuplicatedPolicy : IUnitIdentifierDuplicatedPolicy
{
    private readonly IUnitRepository _repository;

    public UnitIdentifierDuplicatedPolicy(IUnitRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> IsValidAsync(string identifier, CancellationToken cancellationToken)
    {
        return !await _repository.ExistsIdentifierAsync(identifier, cancellationToken);
    }
}

