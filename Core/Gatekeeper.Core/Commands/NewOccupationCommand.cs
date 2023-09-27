using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Commands;

public record NewOccupationCommand(
    Unit Unit,
    List<PersonalInfo> People
);