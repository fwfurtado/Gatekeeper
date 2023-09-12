namespace Gatekeeper.Core.Specifications;

public interface ICpfSpecification : ISpecification<string>
{
}

public class CpfSpecification : ICpfSpecification
{
    private static readonly int[] FirstMultipliers = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
    private static readonly int[] SecondMultipliers = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

    public bool IsValid(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        if (!value.All(char.IsDigit))
        {
            return false;
        }

        if (value.Length != 11)
        {
            return false;
        }

        var cpf = value[..9];
        var firstDigit = CalculateDigit(cpf, FirstMultipliers);
        var secondDigit = CalculateDigit(cpf + firstDigit, SecondMultipliers);

        return string.Concat(firstDigit, secondDigit) == value[9..];
    }

    private static int CalculateDigit(string cpf, IEnumerable<int> multipliers)
    {
        var sum = multipliers.Select((t, i) => int.Parse(cpf[i].ToString()) * t).Sum();

        var mod = sum % 11;

        return mod < 2 ? 0 : 11 - mod;
    }
}