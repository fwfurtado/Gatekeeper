namespace Gatekeeper.Core.Entities;

public class Unit
{
    public string Identifier { get; private set; }
    public List<Resident> Residents { get; }

    public Unit(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            throw new ArgumentException("Identifier cannot be blank");
        }

        Identifier = identifier;
        Residents = new List<Resident>();
    }

    public void AssociateResident(Resident resident)
    {
        Residents.Add(resident);
    }
    
}