namespace DeviceControlService.Domain.Entities;

public record Device
{
    public required string MacAddress { get; init; } 
    public required string Name { get; init; }
    public bool HasInternet { get; set; }
}

