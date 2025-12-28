using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eQuantic.Core.Eventing.Kafka;

/// <summary>
/// Extension methods for registering Kafka event services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Kafka event publisher to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Action to configure options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddKafkaPublisher(
        this IServiceCollection services,
        Action<KafkaEventPublisherOptions> configure)
    {
        services.Configure(configure);

        services.AddSingleton<IProducer<string, string>>(sp =>
        {
            var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<KafkaEventPublisherOptions>>().Value;
            
            var config = options.ProducerConfig ?? new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers,
                Acks = options.Acks
            };

            return new ProducerBuilder<string, string>(config).Build();
        });

        services.AddSingleton<IExternalEventPublisher, KafkaEventPublisher>();

        return services;
    }

    /// <summary>
    /// Adds Kafka event subscriber to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Action to configure options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddKafkaSubscriber(
        this IServiceCollection services,
        Action<KafkaEventSubscriberOptions> configure)
    {
        services.Configure(configure);

        services.AddSingleton<IConsumer<string, string>>(sp =>
        {
            var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<KafkaEventSubscriberOptions>>().Value;
            
            var config = options.ConsumerConfig ?? new ConsumerConfig
            {
                BootstrapServers = options.BootstrapServers,
                GroupId = options.GroupId,
                AutoOffsetReset = options.AutoOffsetReset,
                EnableAutoCommit = options.EnableAutoCommit
            };

            return new ConsumerBuilder<string, string>(config).Build();
        });

        services.AddSingleton<IExternalEventSubscriber, KafkaEventSubscriber>();
        services.AddHostedService<KafkaSubscriberHostedService>();

        return services;
    }
}

/// <summary>
/// Hosted service that manages the Kafka subscriber lifecycle.
/// </summary>
public class KafkaSubscriberHostedService : IHostedService
{
    private readonly IExternalEventSubscriber _subscriber;

    public KafkaSubscriberHostedService(IExternalEventSubscriber subscriber)
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
