namespace Gatekeeper.Frontend.Admin.Dtos;

public class PagedResponse<T>
{
    public IEnumerable<T> Data { get; set; }
    public int Total { get; set; }
    public long Count { get; set; }
}