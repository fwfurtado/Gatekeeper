using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Commands;

public record RegisterPackageCommand(string Description, long UnitId);
