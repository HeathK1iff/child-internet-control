using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DeviceControlService.Domain.Abstractions;
using DeviceControlService.Domain.Services;
using DeviceControlService.Infrastructure.Abstractions;
using DeviceControlService.Infrastructure.Clients;
using System.Net;

namespace DeviceControlService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddTransient<IDevicesRepository, KeeneticDevicesRepository>();
        services.AddTransient<IActivateInternetDeviceService, ActivateInternetDeviceService>();
        services.Configure<KeeneticOptions>(configuration.GetSection(KeeneticOptions.SectionName));

        return services;
    }

    public static IServiceCollection AddKeeneticHttpClient(this IServiceCollection services, IConfiguration configuration)
    {    
        services.AddHttpClient<IKeeneticHttpClient, KeeneticHttpClient>(c =>
        {
            var options = new KeeneticOptions();
            var section = configuration.GetSection(KeeneticOptions.SectionName);
            section.Bind(options);

            c.BaseAddress = new UriBuilder()
            {
                Host = options.Host
            }.Uri;
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
            new HttpClientHandler
                {
                    CookieContainer = new CookieContainer(),
                    AllowAutoRedirect = false,
                    UseCookies = true
                }
        );

        return services;
    }
}


