using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace eQuantic.Core.Eventing.Azure;

/// <summary>
/// Extension methods for registering Azure Service Bus event publisher.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Azure Service Bus event publisher to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Action to configure options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddAzureServiceBusPublisher(
        this IServiceCollection services,
        Action<ServiceBusEventPublisherOptions> configure)
    {
        services.Configure(configure);

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<ServiceBusEventPublisherOptions>>().Value;
            
            if (string.IsNullOrEmpty(options.ConnectionString))
                throw new InvalidOperationException("Azure Service Bus connection string is required.");

            return new ServiceBusClient(options.ConnectionString);
        });

        services.AddSingleton<IExternalEventPublisher, ServiceBusEventPublisher>();

        return services;
    }
}
