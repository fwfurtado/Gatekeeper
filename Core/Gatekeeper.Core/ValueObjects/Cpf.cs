using Gatekeeper.Core.Policies;

namespace Gatekeeper.Core.ValueObjects;

public record Cpf(string Value)
{
    public static implicit operator Cpf(string value) => new(value);

    public static Cpf Parse(string cpf, ICpfPolicy? policy = null)
    {
        policy ??= new CpfPolicy();
        
        policy.IsValid(cpf);
        
        return new Cpf(cpf);
        
    }
};