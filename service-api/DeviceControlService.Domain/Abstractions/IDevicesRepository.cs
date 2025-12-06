using DeviceControlService.Domain.Entities;

public interface IDevicesRepository
{
    Task<IEnumerable<Device>> GetAllAsync(CancellationToken cancellationToken);

    Task<Device> UpdateAsync(Device device, CancellationToken cancellationToken);
    
}