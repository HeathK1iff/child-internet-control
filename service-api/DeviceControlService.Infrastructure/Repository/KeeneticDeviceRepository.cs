using Microsoft.Extensions.Logging;
using DeviceControlService.Domain.Entities;
using DeviceControlService.Infrastructure.Abstractions;
using DeviceControlService.Infrastructure.Dto;

public sealed class KeeneticDevicesRepository(IKeeneticHttpClient httpClient, ILogger<KeeneticDevicesRepository> logger) : IDevicesRepository
{
    readonly IKeeneticHttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    readonly ILogger<KeeneticDevicesRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<IEnumerable<Device>> GetAllAsync(CancellationToken cancellationToken)
    {
        var knowHosts = await _httpClient.GetKnownHostsAsync(cancellationToken);
        var hotSpots = await _httpClient.GetHotspotHostsAsync(cancellationToken);

        var hotSpotsDict = hotSpots.ToDictionary(k => k.MacAddress);

        var ret = new List<Device>();
        foreach (var knowHost in knowHosts)
        {
            if (!hotSpotsDict.TryGetValue(knowHost.MacAddress, out var hotSpot))
            {
                _logger.LogWarning($"Hot spot {knowHost.MacAddress} is not found");
                continue;
            }

            ret.Add(new Device()
            {
                MacAddress = knowHost.MacAddress,
                Name = knowHost.Name,
                HasInternet = (hotSpot.Access, hotSpot.Permit) switch
                {
                    ("permit", true) => true,
                    ("deny", false) => false,
                    _ => false
                }
            });
        }

        return ret;
    }

    public async Task<Device> UpdateAsync(Device device, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateKnowHostsAsync(new KnownHostDto(Name: device.Name, MacAddress: device.MacAddress), cancellationToken);

        var access = device.HasInternet ? "permit" : "deny";
        var hotspotHostDto = new HotspotHostDto(device.MacAddress, access, null, null, null);

        await _httpClient.UpdateHotspotHostAsync(hotspotHostDto, cancellationToken);

        return device;  
    }



}


