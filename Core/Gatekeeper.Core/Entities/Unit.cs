namespace Gatekeeper.Core.Entities;

public class Unit
{
    public long Id { get; set; }
    public string Identifier { get; private set; }

    public Occupation? Occupation { get; private set; }

    public bool IsEmpty => Occupation is null;
    public bool IsOccupied => !IsEmpty;
    
    public Unit(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            throw new ArgumentException("Identifier cannot be blank");
        }

        Identifier = identifier;
    }
    
    public Unit(long unitId, string identifier) : this(identifier)
    {
        Id = unitId;
    }

    public void OccupiedBy(Occupation occupation)
    {
        if (IsOccupied)
        {
            throw new InvalidOperationException("Unit is already occupied");
        }
        
        Occupation = occupation;
    }
}