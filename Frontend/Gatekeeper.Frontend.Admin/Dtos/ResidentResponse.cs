using MudBlazor;

namespace Gatekeeper.Frontend.Admin.Dtos;

public class ResidentResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = FilterOperator.String.Empty;
    public string Document { get; set; } = FilterOperator.String.Empty;
}