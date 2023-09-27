using Gatekeeper.Core.ValueObjects;

namespace Gatekeeper.Core.Entities;

public class PersonalInfo
{
    public required string Name { get; set; }
    public required Cpf Document { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
}