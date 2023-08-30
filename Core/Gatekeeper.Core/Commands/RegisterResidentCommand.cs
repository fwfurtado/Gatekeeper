using Gatekeeper.Core.ValueObjects;

namespace Gatekeeper.Core.Commands;

public record RegisterResidentCommand(string Name, Cpf Document);