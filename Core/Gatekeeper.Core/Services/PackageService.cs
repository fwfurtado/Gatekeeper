using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Repositories;
using Gatekeeper.Core.ValueObjects;
using MediatR;

namespace Gatekeeper.Core.Services;

public class PackageService : IPackageService
{
    private readonly IPackageRepository _repository;
    private readonly IValidator<RegisterPackageCommand> _registerPackageValidator;
    private readonly IMapper _mapper;

    public PackageService(
        IPackageRepository repository, 
        IValidator<RegisterPackageCommand> registerPackageValidator, 
        IMapper mapper)
    {
        _repository = repository;
        _registerPackageValidator = registerPackageValidator;
        _mapper = mapper;
    }
    
    public async Task DeletePackage(long packageId, CancellationToken cancellationToken)
    {
        await _repository.DeleteByIdAsync(packageId, cancellationToken);
    }

    public async Task<PagedList<Package>> GetAllPackages(PageRequest pageRequest, CancellationToken cancellationToken)
    {
        return await _repository.GetAll(pageRequest, cancellationToken);
    }

    public Task<Package?> GetPackageByIdAsync(long packageId, CancellationToken cancellationToken)
    {
        return _repository.GetByIdAsync(packageId, cancellationToken);
    }

    public async Task UpdateStatusDeliveredAsync(long packageId, CancellationToken cancellationToken)
    {
        var package = await _repository.GetByIdAsync(packageId, cancellationToken);
        if(package is not null)
        {
            package.Deliver();
            await _repository.UpdateStatus(package.Id, PackageStatus.Delivered, cancellationToken);
        }
        
    }

    public async Task UpdateStatusRejectedAsync(long packageId, CancellationToken cancellationToken)
    {
        var package = await _repository.GetByIdAsync(packageId, cancellationToken);
        if (package is not null)
        {
            package.Reject();
            await _repository.UpdateStatus(package.Id, PackageStatus.Rejected, cancellationToken);
        }
    }
}
