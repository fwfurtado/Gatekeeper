using System.Transactions;
using AutoMapper;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Repositories;
using MediatR;
using Unit = Gatekeeper.Core.Entities.Unit;

namespace Gatekeeper.Core.Services;

public class OccupationService : IOccupationService
{
    private readonly IOccupationRequestRepository _requestRepository;
    private readonly IUnitRepository _unitRepository;
    private readonly IOccupationRequestEffectiveUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;

    public OccupationService(
        IOccupationRequestEffectiveUnitOfWork unitOfWork,
        IUnitRepository unitRepository,
        IOccupationRequestRepository requestRepository,
        IMapper mapper,
        IPublisher publisher
    )
    {
        _unitOfWork = unitOfWork;
        _unitRepository = unitRepository;
        _requestRepository = requestRepository;
        _mapper = mapper;
        _publisher = publisher;
        _unitOfWork = unitOfWork;
    }

    public async Task RequestOccupationAsync(NewOccupationCommand command, CancellationToken cancellationToken)
    {
        var request = _mapper.Map<OccupationRequest>(command);

        await _requestRepository.SaveAsync(request, cancellationToken);
    }


    public async Task<OccupationRequest?> GetById(long id, CancellationToken cancellationToken)
    {
        return await _requestRepository.GetRequestByIdAsync(id, cancellationToken);
    }

    public async Task ApproveRequestAsync(long requestId, CancellationToken cancellationToken)
    {
        var request = await _requestRepository.GetRequestByIdAsync(requestId, cancellationToken) ??
                      throw new InvalidOperationException("Request not found");

        var approved = request.Approve();

        await _requestRepository.UpdateRequestStatusAsync(request, cancellationToken);

        await _publisher.Publish(approved, cancellationToken);
    }

    public async Task RejectRequestAsync(long requestId, string reason, CancellationToken cancellationToken)
    {
        var request = await _requestRepository.GetRequestByIdAsync(requestId, cancellationToken) ??
                      throw new InvalidOperationException("Request not found");

        var rejected = request.Reject(reason);

        await _requestRepository.UpdateRequestStatusAsync(request, cancellationToken);

        await _publisher.Publish(rejected, cancellationToken);
    }

    public async Task EffectiveApprovedRequest(long requestId, CancellationToken cancellationToken)
    {
        var request = await _requestRepository.GetRequestByIdAsync(requestId, cancellationToken) ??
                      throw new InvalidOperationException("Request not found");

        if (request.IsNotApproved)
        {
            throw new InvalidOperationException("Request is not approved");
        }

        var unit = request.Unit;

        var occupation = new Occupation
        {
            Unit = request.Unit,
            Residents = request.People.Select(personalInfo => _mapper.Map<Resident>(personalInfo)).ToList()
        };

        unit.OccupiedBy(occupation);

        using var work = _unitOfWork;

        await work.SaveUnitAsync(unit, cancellationToken);
        await work.SaveOccupationAsync(occupation, cancellationToken);

        work.Commit();
    }
}

public interface IOccupationRequestEffectiveUnitOfWork : IDisposable
{
    public Task SaveOccupationAsync(Occupation occupation, CancellationToken cancellationToken);
    public Task SaveUnitAsync(Unit unit, CancellationToken cancellationToken);
    void Commit();
}

public class OccupationRequestEffectiveUnitOfWork : IOccupationRequestEffectiveUnitOfWork
{
    private readonly IUnitRepository _unitRepository;
    private readonly IOccupationRepository _occupationRepository;
    private readonly TransactionScope _scope = new();

    public OccupationRequestEffectiveUnitOfWork(IOccupationRepository occupationRepository,
        IUnitRepository unitRepository)
    {
        _occupationRepository = occupationRepository;
        _unitRepository = unitRepository;
    }

    public async Task SaveOccupationAsync(Occupation occupation, CancellationToken cancellationToken)
    {
        await _occupationRepository.SaveOccupationAsync(occupation, cancellationToken);
    }

    public async Task SaveUnitAsync(Unit unit, CancellationToken cancellationToken)
    {
        await _unitRepository.UpdateOccupationAsync(unit, cancellationToken);
    }

    public void Commit()
    {
        _scope.Complete();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _scope.Dispose();
        }
    }
}