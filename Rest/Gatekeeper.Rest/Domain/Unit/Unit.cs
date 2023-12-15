namespace Gatekeeper.Rest.Domain.Unit;

public class Unit
{
    public long Id { get; set; }
    public string Identifier { get; private set; }

    public Domain.Occupation.Occupation? Occupation { get; private set; }

    public bool IsEmpty => Occupation is null;
    public bool IsOccupied => !IsEmpty;
}
