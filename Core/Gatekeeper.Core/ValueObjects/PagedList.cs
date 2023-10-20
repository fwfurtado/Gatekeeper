namespace Gatekeeper.Core.ValueObjects;

public class PagedList<T>
{
    public required IList<T> Data { get; init; }

    public required int Total { get; init; }
    
    public int Count => Data.Count;
    
    public PagedList<TDestination> Select<TDestination>(Func<T, TDestination> map)
    {
        return new PagedList<TDestination>
        {
            Data = Data.Select(map).ToList(),
            Total = Total
        };
    }
}