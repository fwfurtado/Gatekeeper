﻿using Gatekeeper.Core.Entities;
using System;
using System.Text.Json.Serialization;
using Gatekeeper.Rest.Domain.Aggregate;

namespace Gatekeeper.Rest.Dtos;

public class PackageResponse
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; } = null!;

    [JsonPropertyName("arrived_at")] public DateTime ArrivedAt { get; set; }

    [JsonPropertyName("delivered_at")] public DateTime DeliveredAt { get; set; }
    [JsonPropertyName("status")] public PackageStatus Status { get; set; }
    [JsonPropertyName("unit_id")] public long UnitId { get; set; }
}
