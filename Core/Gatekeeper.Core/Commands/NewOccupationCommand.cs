using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Commands;

public record NewOccupationCommand(
    TargetUnit Unit,
    List<PersonalInfo> People
);