namespace Gatekeeper.Core.Entities;

public class Unit
{
    public string Identifier { get; private set; }
    public List<Tenant> Tenants { get; }

    public Unit(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            throw new ArgumentException("Identifier cannot be blank");
        }

        Identifier = identifier;
        Tenants = new List<Tenant>();
    }

    public void AddTenant(Tenant tenant)
    {
        Tenants.Add(tenant);
    }
    
}