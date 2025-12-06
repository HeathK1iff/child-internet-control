using System.Runtime.Serialization;

namespace DeviceControlService.Domain.Exceptions;

public sealed class NotFoundDeviceException : DomainException
{
    public NotFoundDeviceException()
    {
    }

    public NotFoundDeviceException(string? message) : base(message)
    {
    }

    public NotFoundDeviceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public NotFoundDeviceException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
