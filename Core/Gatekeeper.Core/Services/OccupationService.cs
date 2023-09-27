using AutoMapper;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Exceptions;
using Gatekeeper.Core.Repositories;
using MediatR;

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
                      throw new OccupationRequestNotFouncException("Request not found");

        var approved = request.Approve();

        await _requestRepository.UpdateRequestStatusAsync(request, cancellationToken);

        await _publisher.Publish(approved, cancellationToken);
    }

    public async Task RejectRequestAsync(long requestId, string reason, CancellationToken cancellationToken)
    {
        var request = await _requestRepository.GetRequestByIdAsync(requestId, cancellationToken) ??
                      throw new OccupationRequestNotFouncException("Request not found");

        var rejected = request.Reject(reason);

        await _requestRepository.UpdateRequestStatusAsync(request, cancellationToken);

        await _publisher.Publish(rejected, cancellationToken);
    }

    public async Task EffectiveApprovedRequest(long requestId, CancellationToken cancellationToken)
    {
        var request = await _requestRepository.GetRequestByIdAsync(requestId, cancellationToken) ??
                      throw new OccupationRequestNotFouncException("Request not found");

        if (request.IsNotApproved)
        {
            throw new InvalidOperationException("Request is not approved");
        }


        var unit = await _unitRepository.GetByIdAsync(request.Unit.UnitId, cancellationToken) ??
                   throw new InvalidOperationException("Unit not found");

        var occupation = new Occupation
        {
            Unit = request.Unit,
            Residents = request.People.Select(personalInfo => _mapper.Map<Resident>(personalInfo)).ToList(),
            Start = DateOnly.FromDateTime(DateTime.UtcNow),
            End = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(2)
        };

        unit.OccupiedBy(occupation);

        await _unitOfWork.CreateOccupationAndAssociateWithUnit(occupation, unit, cancellationToken);
    }
}