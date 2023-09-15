namespace Gatekeeper.Core.Entities;

public class Occupation
{
    public long Id { get; set; }
    public List<Resident> Residents { get; set; } = new();
    public required Unit Unit { get; set; }

    public DateOnly? Start { get; set; }
    public DateOnly? End { get; set; }
}