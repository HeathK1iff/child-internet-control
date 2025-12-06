using DeviceControlService.Domain.Entities;

namespace DeviceControlService.Domain.Abstractions;

public interface IActivateInternetDeviceService
{
    Task<IEnumerable<Device>> GetClientsAsync(CancellationToken cancellationToken);
    Task<Device> ActivateInternetForClientAsync(string mac, bool activate, CancellationToken cancellationToken);
}
