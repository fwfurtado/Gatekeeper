using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MediatR;

namespace Gatekeeper.Rest.Features.Package.Receive;

public record ReceivePackageCommand(long UnitId, string Description) : IRequest<long>;

public class ReceivePackageService(
    IPackageFetcherByDescription fetcherByDescription,
    IPackageSaver packageSaver,
    IValidator<ReceivePackageCommand> validator
) : IRequestHandler<ReceivePackageCommand, long>
{
    public async Task<long> Handle(ReceivePackageCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        if (await fetcherByDescription.ExistsDescriptionAsync(command.Description, cancellationToken))
        {
            var failure = new ValidationFailure("Description", "Description already exists");
            throw new ValidationException(new[] { failure });
        }

        var package = command.Adapt<Domain.Package>();

        cancellationToken.ThrowIfCancellationRequested();

        var id = await packageSaver.SaveAsync(package, cancellationToken);

        return id;
    }
}