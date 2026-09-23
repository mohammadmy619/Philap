using Microsoft.Extensions.Configuration;
using Ocelot.Configuration;
using Ocelot.ServiceDiscovery.Providers;
using Ocelot.Values;

namespace Ocelot.ApiGateway.Extensions;

public class AspireServiceDiscoveryProvider : IServiceDiscoveryProvider
{
    private readonly IConfiguration _configuration;
    private readonly DownstreamRoute _route;

    public AspireServiceDiscoveryProvider(
        IServiceProvider serviceProvider,
        ServiceProviderConfiguration config,
        DownstreamRoute route)
    {
        _configuration = serviceProvider.GetRequiredService<IConfiguration>();
        _route = route;
    }

    public Task<List<Service>> GetAsync()
    {
        var serviceName = _route.ServiceName;

        if (string.IsNullOrWhiteSpace(serviceName))
        {
            throw new InvalidOperationException(
                "ServiceName is missing in ocelot.json route. " +
                "Set \"ServiceName\": \"identity-api\" (or person-api, trip-api, ...).");
        }

        // Aspire این کلیدها را تزریق می‌کند:
        // services__identity-api__http__0
        // services__identity-api__https__0
        // services__person-api__http__0
        // ...
        var endpoint =
            _configuration[$"services:{serviceName}:http:0"] ??
            _configuration[$"services:{serviceName}:https:0"];

        if (string.IsNullOrWhiteSpace(endpoint))
        {
            throw new InvalidOperationException(
                $"Service '{serviceName}' not found in Aspire service discovery. " +
                $"Add .WithReference(...) for this service in AppHost.");
        }

        var uri = new Uri(endpoint);

        var service = new Service(
            name: serviceName,
            hostAndPort: new ServiceHostAndPort(uri.Host, uri.Port),
            id: $"{serviceName}-{uri.Port}",
            version: "1.0",
            tags: []);

        return Task.FromResult(new List<Service> { service });
    }
}