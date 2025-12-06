using System.Text.Json.Serialization;

namespace DeviceControlService.Application.Responses;

public record ApiResponse<TResult>([property: JsonPropertyName("Message")] string Message, [property:JsonPropertyName("Payload")] TResult? Payload);