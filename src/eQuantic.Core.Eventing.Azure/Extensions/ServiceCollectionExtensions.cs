using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eQuantic.Core.Eventing.Azure;

/// <summary>
/// Extension methods for registering Azure Service Bus event services.
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

    /// <summary>
    /// Adds Azure Service Bus event subscriber to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Action to configure options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddAzureServiceBusSubscriber(
        this IServiceCollection services,
        Action<ServiceBusEventSubscriberOptions> configure)
    {
        services.Configure(configure);

        // Reuse or create ServiceBusClient
        services.AddSingleton(sp =>
        {
            var publisherOptions = sp.GetService<Microsoft.Extensions.Options.IOptions<ServiceBusEventPublisherOptions>>()?.Value;
            var subscriberOptions = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<ServiceBusEventSubscriberOptions>>().Value;
            
            var connectionString = publisherOptions?.ConnectionString ?? subscriberOptions.ConnectionString;
            
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Azure Service Bus connection string is required.");

            return new ServiceBusClient(connectionString);
        });

        services.AddSingleton<IExternalEventSubscriber, ServiceBusEventSubscriber>();
        services.AddHostedService<ServiceBusSubscriberHostedService>();

        return services;
    }
}

/// <summary>
/// Hosted service that manages the Service Bus subscriber lifecycle.
/// </summary>
public class ServiceBusSubscriberHostedService : IHostedService
{
    private readonly IExternalEventSubscriber _subscriber;

    public ServiceBusSubscriberHostedService(IExternalEventSubscriber subscriber)
    {
        _subscriber = subscriber;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _subscriber.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _subscriber.StopAsync(cancellationToken);
    }
}
