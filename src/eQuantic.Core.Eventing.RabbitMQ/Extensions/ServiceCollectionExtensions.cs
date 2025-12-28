using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace eQuantic.Core.Eventing.RabbitMQ;

/// <summary>
/// Extension methods for registering RabbitMQ event publisher.
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
}
