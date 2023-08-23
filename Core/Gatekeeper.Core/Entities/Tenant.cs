namespace Gatekeeper.Core.Entities;

public class Tenant
{
    public  string Name { get; private set; }
    public string Document { get; private set; }

    public Tenant(string name, string document)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name cannot be blank");
        }
        if (string.IsNullOrEmpty(document))
        {
            throw new ArgumentException("Document cannot be blank");
        }
    
        Name = name;
        Document = document;
    }
}