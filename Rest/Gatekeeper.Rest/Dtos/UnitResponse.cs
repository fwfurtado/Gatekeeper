using System.Text.Json.Serialization;

namespace Gatekeeper.Rest.Dtos;

public class UnitResponse
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("identifier")] public string Identifier { get; set; } = null!;
}