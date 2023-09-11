using Gatekeeper.Core.ValueObjects;

namespace Gatekeeper.Core.Entities;

public class Resident
{
    public string Name { get; private set; }
    public Cpf Document { get; private set; }
    public long Id { get; private set; }

    public Resident(string name, Cpf document)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name cannot be blank");
        }

        Name = name;
        Document = document;
    }
    public Resident(long id, string document, string name) : this(name, document)
    {
        Id = id;
    }
}