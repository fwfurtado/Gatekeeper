namespace Gatekeeper.Core.Entities;

public class Unit
{
    private readonly List<Resident> _residents; 
    public string Identifier { get; private set; }
    public IReadOnlyList<Resident> Residents => _residents;

    public Unit(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            throw new ArgumentException("Identifier cannot be blank");
        }

        Identifier = identifier;
        _residents = new List<Resident>();
    }

    public void AssociateResident(Resident resident)
    {
        _residents.Add(resident);
    }
    
}