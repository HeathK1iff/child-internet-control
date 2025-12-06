using DeviceControlService.Domain.Abstractions;
using DeviceControlService.Domain.Entities;
using DeviceControlService.Domain.Exceptions;

namespace DeviceControlService.Domain.Services;

public class ActivateInternetDeviceService(IDevicesRepository clientsRepository): IActivateInternetDeviceService
{
    readonly IDevicesRepository _clientsRepository = clientsRepository ?? throw new ArgumentNullException(nameof(clientsRepository));

    public async Task<IEnumerable<Device>> GetClientsAsync(CancellationToken cancellationToken)
    {
        return await _clientsRepository.GetAllAsync(cancellationToken);
    }

    public async Task<Device> ActivateInternetForClientAsync(string mac, bool activate, CancellationToken cancellationToken)
    {
        var devices = await _clientsRepository.GetAllAsync(cancellationToken);

        var foundClient = devices.FirstOrDefault(p => p.MacAddress == mac);

        if (foundClient is null)
        {
            throw new NotFoundDeviceException($"Device is not found {mac}");
        }

        foundClient.HasInternet = activate;
        
        await _clientsRepository.UpdateAsync(foundClient, cancellationToken);

        return foundClient;
    }
}
