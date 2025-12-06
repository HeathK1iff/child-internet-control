using DeviceControlService.Infrastructure.Dto;

namespace DeviceControlService.Infrastructure.Abstractions;

public interface IKeeneticHttpClient
{
    Task<IEnumerable<KnownHostDto>> GetKnownHostsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<HotspotHostDto>> GetHotspotHostsAsync(CancellationToken cancellationToken);

    Task UpdateKnowHostsAsync(KnownHostDto knownHostDto, CancellationToken cancellationToken);
    Task UpdateHotspotHostAsync(HotspotHostDto knownHostDto, CancellationToken cancellationToken);
}

