namespace Gatekeeper.Core.Entities;

public class Apartment
{
    public string Identifier { get; private set; }
    public List<Tenant> Tenants { get; } = new();
    
    public Apartment(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            throw new ArgumentException("Identifier cannot be blank");
        }

        Identifier = identifier;
    }
    
}