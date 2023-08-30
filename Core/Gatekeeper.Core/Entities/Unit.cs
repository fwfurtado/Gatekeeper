using System.Collections.ObjectModel;

namespace Gatekeeper.Core.Entities;

public class Unit
{
    private readonly List<Resident> _residents; 
    public string Identifier { get; private set; }
    public IEnumerable<Resident> Residents => _residents.AsReadOnly();

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