using Microsoft.Extensions.Options;
using DeviceControlService.Infrastructure.Abstractions;
using DeviceControlService.Infrastructure.Dto;
using DeviceControlService.Infrastructure.Helpers;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DeviceControlService.Infrastructure.Clients;

public sealed class KeeneticHttpClient(HttpClient httpClient, IOptions<KeeneticOptions> options) : IKeeneticHttpClient
{
    static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    readonly KeeneticOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

    private async Task<HttpResponseMessage> SendRequestAsync(Func<HttpClient, Task<HttpResponseMessage>> action, CancellationToken cancellationToken) 
    {
        var response = await action.Invoke(_httpClient);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
        {
            return response;
        }

        response = await _httpClient.GetAsync("/auth", cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
        {
            return response;
        }

        string realm = AuthenticationHeaderExtractHelper.ExtractRealm(response.Headers);
        string realmChallenge = AuthenticationHeaderExtractHelper.ExtractRealmChallenge(response.Headers);

        var cryptedPassword = PasswordCryptHelper.GeneratePassword(_options.Credential.Login, _options.Credential.Password,
            realm, realmChallenge);

        var credential = new CredentialDto(_options.Credential.Login, cryptedPassword);

        

        var jsonContent = JsonSerializer.Serialize(credential, _jsonOptions);
        HttpContent content = new StringContent(jsonContent, Encoding.ASCII, "application/json");

        var response2 = await _httpClient.PostAsync("/auth", content);

        response2.EnsureSuccessStatusCode();

        return  await action.Invoke(_httpClient);
    }

    public async Task<IEnumerable<KnownHostDto>> GetKnownHostsAsync(CancellationToken cancellationToken)
    {    
        var response = await SendRequestAsync(client => client.GetAsync("/rci/known/host", cancellationToken), cancellationToken);
    

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return Enumerable.Empty<KnownHostDto>();    
        }

        var ret = await response.Content.ReadFromJsonAsync<IDictionary<string, MacAddressDto>>(_jsonOptions) ?? new Dictionary<string, MacAddressDto>();

        return ret
            .Select(kv => new KnownHostDto(Name: kv.Key, MacAddress: kv.Value.MacAddress ?? string.Empty))
            .ToArray();
    }

    public async Task<IEnumerable<HotspotHostDto>> GetHotspotHostsAsync(CancellationToken cancellationToken)
    {
        var response = await SendRequestAsync(client => client.GetAsync("/rci/ip/hotspot/host", cancellationToken), cancellationToken);
    
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return Enumerable.Empty<HotspotHostDto>();    
        }

        return await response.Content.ReadFromJsonAsync<IEnumerable<HotspotHostDto>>() ?? [];
    }

    public async Task UpdateKnowHostsAsync(KnownHostDto knownHostDto, CancellationToken cancellationToken)
    {
        KnownHostDto[] request = [
            knownHostDto
        ];

        var response = await SendRequestAsync(client => client.PostAsJsonAsync("/rci/known/host", request, _jsonOptions, cancellationToken), cancellationToken);
    
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateHotspotHostAsync(HotspotHostDto knownHotspotHostDto, CancellationToken cancellationToken)
    {
        HotspotHostDto[] request = [
            knownHotspotHostDto
        ];

        var response = await SendRequestAsync(client => client.PostAsJsonAsync("/rci/ip/hotspot/host", request, _jsonOptions, cancellationToken), cancellationToken);
    
        response.EnsureSuccessStatusCode();
    }

    public record MacAddressDto([property: JsonPropertyName("mac")] string MacAddress);

    public record CredentialDto([property: JsonPropertyName("login")] string Login, 
        [property: JsonPropertyName("password")] string Password);
}

