using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Gatekeeper.Rest.Features.Package.Receive;

public class ReceivePackageService(IMapper mapper, IReceiveRepository repository, IValidator<ReceivePackageCommand> validator) : IRequestHandler<ReceivePackageCommand, long>
{
    public async Task<long> Handle(ReceivePackageCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        if (await repository.ExistsDescriptionAsync(command.Description, cancellationToken))
        {
            var failure = new ValidationFailure("Description", "Description already exists");
            throw new ValidationException(new[] { failure });
        }

        var package = mapper.Map<ReceivedPackage>(command);

        cancellationToken.ThrowIfCancellationRequested();

        var id = await repository.SaveAsync(package, cancellationToken);

        return id;
    }
}