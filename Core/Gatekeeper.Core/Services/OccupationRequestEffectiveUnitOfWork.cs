using System.Transactions;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Repositories;
using Gatekeeper.Shared.Database;

namespace Gatekeeper.Core.Services;

public class OccupationRequestEffectiveUnitOfWork : IOccupationRequestEffectiveUnitOfWork
{
    private readonly IUnitRepository _unitRepository;
    private readonly IOccupationRepository _occupationRepository;
    private readonly IDbConnectionFactory _connectionFactory;

    public OccupationRequestEffectiveUnitOfWork(IOccupationRepository occupationRepository,
        IUnitRepository unitRepository, IDbConnectionFactory connectionFactory)
    {
        _occupationRepository = occupationRepository;
        _unitRepository = unitRepository;
        _connectionFactory = connectionFactory;
    }

    public async Task CreateOccupationAndAssociateWithUnit(Occupation occupation, Unit unit,
        CancellationToken cancellationToken)
    {
        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        using var conn = _connectionFactory.CreateConnection();

        await _occupationRepository.SaveOccupationAsync(occupation, cancellationToken);
        await _unitRepository.UpdateOccupationAsync(unit, cancellationToken);

        tx.Complete();
    }
}