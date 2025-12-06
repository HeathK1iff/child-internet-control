using System.Runtime.Serialization;

namespace DeviceControlService.Domain.Exceptions;

public abstract class DomainException : Exception
{
    public DomainException()
    {
    }

    public DomainException(string? message) : base(message)
    {
    }

    public DomainException(SerializationInfo info, StreamingContext context) 
    {
    }

    public DomainException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
