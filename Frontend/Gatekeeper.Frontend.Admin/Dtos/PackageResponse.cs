using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Gatekeeper.Frontend.Admin.Dtos;

public class PackageResponse
{
    public long Id { get; set; }
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("unit_id")] public long UnitId { get; set; }
    [JsonPropertyName("arrived_at")] public DateTime ArrivedAt { get; set; }
    [JsonPropertyName("delivered_at")] public DateTime? DeliveredAt { get; set; }
    public string Status { get; set; } = string.Empty;

    public bool CanDeliver => Status == "Pending";
    public bool CanReject => Status == "Pending";
}