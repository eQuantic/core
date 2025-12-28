using Microsoft.Extensions.DependencyInjection;

namespace eQuantic.Core.Eventing.Outbox;

/// <summary>
/// Extension methods for registering outbox services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds outbox pattern support to the service collection.
    /// </summary>
    /// <typeparam name="TStore">The outbox store implementation type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Action to configure options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddOutbox<TStore>(
        this IServiceCollection services,
        Action<OutboxOptions>? configure = null)
        where TStore : class, IOutboxStore
    {
        if (configure != null)
        {
            services.Configure(configure);
        }

        services.AddSingleton<IOutboxStore, TStore>();
        services.AddSingleton<IEventDispatcher, OutboxEventDispatcher>();
        services.AddHostedService<OutboxProcessorService>();

        return services;
    }
}
