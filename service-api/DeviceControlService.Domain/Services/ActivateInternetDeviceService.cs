using DeviceControlService.Domain.Abstractions;
using DeviceControlService.Domain.Entities;
using DeviceControlService.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace DeviceControlService.Domain.Services;

public class ActivateInternetDeviceService(IDevicesRepository clientsRepository, ILogger<ActivateInternetDeviceService> logger): IActivateInternetDeviceService
{
    readonly IDevicesRepository _clientsRepository = clientsRepository ?? throw new ArgumentNullException(nameof(clientsRepository));
    readonly ILogger<ActivateInternetDeviceService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
            _logger.LogError($"Device is not found {mac}");
            throw new NotFoundDeviceException($"Device is not found {mac}");
        }

        foundClient.HasInternet = activate;
        
        _logger.LogInformation($"Device state ({foundClient.Name}) is changed {activate}");

        await _clientsRepository.UpdateAsync(foundClient, cancellationToken);

        return foundClient;
    }
}
