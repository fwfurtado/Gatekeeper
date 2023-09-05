namespace Gatekeeper.Core.ValueObjects;

public record Cpf(string Number)
{
    public static implicit operator Cpf(string value) => new(value);
    public static implicit operator string(Cpf value) => value.Number;
};