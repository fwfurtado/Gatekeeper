using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace Gatekeeper.Frontend.Admin.Dtos;

public class PackageResponse
{
    public long Id { get; set; }
    public string Description { get; set; } = FilterOperator.String.Empty;
    public long UnitId { get; set; }
    public DateTime ArrivedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string Status { get; set; } = FilterOperator.String.Empty;
}