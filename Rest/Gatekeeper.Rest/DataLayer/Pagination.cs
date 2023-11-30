namespace Gatekeeper.Rest.DataLayer;

public readonly struct Pagination(int page, int size)
{
    public int Page => page;
    public int Size => size;
}