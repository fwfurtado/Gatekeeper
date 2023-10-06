using MudBlazor;

namespace Gatekeeper.Frontend.Admin.Dtos;

public class UnitResponse
{
    public int Id { get; set; }
    public string Identifier { get; set; } = FilterOperator.String.Empty;   
}