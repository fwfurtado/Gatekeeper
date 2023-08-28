using Gatekeeper.Core.ValueObjects;

namespace Gatekeeper.Core.Entities;

public class Resident
{
    public  string Name { get; private set; }
    public Cpf Document { get; private set; }

    public Resident(string name, Cpf document)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name cannot be blank");
        }
        
        Name = name;
        Document = document;
    }
}