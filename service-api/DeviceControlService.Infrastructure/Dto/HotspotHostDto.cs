using System.Text.Json.Serialization;

namespace DeviceControlService.Infrastructure.Dto;

public record HotspotHostDto(
    [property: JsonPropertyName("mac")] string MacAddress,
    [property: JsonPropertyName("access")] string Access,
    [property: JsonPropertyName("permit")] bool? Permit,
    [property: JsonPropertyName("deny")] bool? Deny,
    [property: JsonPropertyName("policy")] string? Policy
);