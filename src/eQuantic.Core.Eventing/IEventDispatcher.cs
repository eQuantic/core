namespace eQuantic.Core.Eventing;

/// <summary>
/// Defines an event dispatcher that can publish events to their handlers.
/// </summary>
public interface IEventDispatcher
{
    /// <summary>
    /// Dispatches a single event to all registered handlers.
    /// </summary>
    /// <typeparam name="TEvent">The type of event.</typeparam>
    /// <param name="event">The event to dispatch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task DispatchAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent;

    /// <summary>
    /// Dispatches multiple events to their handlers.
    /// </summary>
    /// <param name="events">The events to dispatch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task DispatchAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken = default);
}

/// <summary>
/// Strategy for dispatching events to multiple handlers.
/// </summary>
public enum EventDispatchStrategy
{
    /// <summary>
    /// Execute all handlers in parallel.
    /// </summary>
    WhenAll,

    /// <summary>
    /// Execute handlers sequentially, stopping at first exception.
    /// </summary>
    StopOnFirstException,

    /// <summary>
    /// Execute all handlers sequentially, collecting all exceptions.
    /// </summary>
    ContinueOnException
}
