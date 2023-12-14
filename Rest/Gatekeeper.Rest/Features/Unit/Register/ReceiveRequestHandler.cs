using FluentValidation;
using Mapster;
using MediatR;

namespace Gatekeeper.Rest.Features.Unit.Register;

public record ReceiveUnitCommand(string Identifier) : IRequest<long>;
public class ReceiveUnitService(
    IUnitSaver unitSaver,
    IValidator<ReceiveUnitCommand> validator
) : IRequestHandler<ReceiveUnitCommand, long>
{
    public async Task<long> Handle(ReceiveUnitCommand command, CancellationToken cancellationToken) 
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        var unit = command.Adapt<Domain.Unit.Unit>();
        cancellationToken.ThrowIfCancellationRequested();
        var id = await unitSaver.SaveAsync(unit, cancellationToken);
        return id;
    }
}
