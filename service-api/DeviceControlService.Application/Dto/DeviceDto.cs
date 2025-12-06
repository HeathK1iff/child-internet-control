using System.Text.Json.Serialization;

namespace DeviceControlService.Application.Dto;

public class DeviceDto
{
    [JsonPropertyName("mac")]
    public required string MacAddress { get; init; } 
    
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    
    [JsonPropertyName("hasInternet")]
    public bool HasInternet { get; set; }
}
