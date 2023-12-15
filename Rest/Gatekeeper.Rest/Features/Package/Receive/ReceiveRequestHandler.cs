using System.Transactions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Gatekeeper.Rest.Features.Package.Receive;

public record ReceivePackageCommand(long UnitId, string Description) : IRequest<long>;

public class ReceivePackageService(
    IPublisher publisher,
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

        var (unitId, description) = command;

        var package = Domain.Package.Package.Factory(description, unitId);

        cancellationToken.ThrowIfCancellationRequested();

        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var receivedEvent = await packageSaver.SaveAsync(package, cancellationToken);

        await publisher.Publish(receivedEvent, cancellationToken);

        tx.Complete();

        return receivedEvent.PackageId;
    }
}