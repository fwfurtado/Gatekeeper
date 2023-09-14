using Gatekeeper.Core.ValueObjects;
using System.Text.Json.Serialization;

namespace Gatekeeper.Rest.Dtos;

public class ResidentResponse
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; } = null!;

    [JsonPropertyName("document")] public string Document { get; set; } = null!;
}