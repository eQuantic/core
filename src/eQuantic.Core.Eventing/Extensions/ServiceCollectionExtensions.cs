using Microsoft.Extensions.DependencyInjection;

namespace eQuantic.Core.Eventing.Extensions;

/// <summary>
/// Extension methods for registering eventing services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the in-memory event dispatcher to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="strategy">The dispatch strategy to use.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddEventDispatcher(
        this IServiceCollection services,
        EventDispatchStrategy strategy = EventDispatchStrategy.WhenAll)
    {
        services.AddSingleton<IEventDispatcher>(sp => 
            new InMemoryEventDispatcher(sp, strategy));
        return services;
    }

    /// <summary>
    /// Registers an event handler for a specific event type.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    /// <typeparam name="THandler">The handler type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddEventHandler<TEvent, THandler>(this IServiceCollection services)
        where TEvent : IEvent
        where THandler : class, IEventHandler<TEvent>
    {
        services.AddTransient<IEventHandler<TEvent>, THandler>();
        return services;
    }
}
