using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace eQuantic.Core.Eventing.RabbitMQ;

/// <summary>
/// Extension methods for registering RabbitMQ event services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds RabbitMQ event publisher to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Action to configure options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddRabbitMqPublisher(
        this IServiceCollection services,
        Action<RabbitMqEventPublisherOptions> configure)
    {
        services.Configure(configure);

        services.AddSingleton<IConnection>(sp =>
        {
            var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<RabbitMqEventPublisherOptions>>().Value;
            
            var factory = new ConnectionFactory
            {
                HostName = options.HostName,
                Port = options.Port,
                UserName = options.UserName,
                Password = options.Password,
                VirtualHost = options.VirtualHost
            };

            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        services.AddSingleton<IExternalEventPublisher, RabbitMqEventPublisher>();

        return services;
    }

    /// <summary>
    /// Adds RabbitMQ event subscriber to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Action to configure options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddRabbitMqSubscriber(
        this IServiceCollection services,
        Action<RabbitMqEventSubscriberOptions> configure)
    {
        services.Configure(configure);

        // Reuse or create connection
        services.AddSingleton<IConnection>(sp =>
        {
            var publisherOptions = sp.GetService<Microsoft.Extensions.Options.IOptions<RabbitMqEventPublisherOptions>>()?.Value;
            var subscriberOptions = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<RabbitMqEventSubscriberOptions>>().Value;
            
            var factory = new ConnectionFactory
            {
                HostName = publisherOptions?.HostName ?? subscriberOptions.HostName,
                Port = publisherOptions?.Port ?? subscriberOptions.Port,
                UserName = publisherOptions?.UserName ?? subscriberOptions.UserName,
                Password = publisherOptions?.Password ?? subscriberOptions.Password,
                VirtualHost = publisherOptions?.VirtualHost ?? subscriberOptions.VirtualHost
            };

            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        services.AddSingleton<IExternalEventSubscriber, RabbitMqEventSubscriber>();
        services.AddHostedService<RabbitMqSubscriberHostedService>();

        return services;
    }
}

/// <summary>
/// Hosted service that manages the RabbitMQ subscriber lifecycle.
/// </summary>
public class RabbitMqSubscriberHostedService : IHostedService
{
    private readonly IExternalEventSubscriber _subscriber;

    public RabbitMqSubscriberHostedService(IExternalEventSubscriber subscriber)
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
