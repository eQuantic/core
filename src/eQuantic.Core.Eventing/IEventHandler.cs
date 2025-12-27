namespace eQuantic.Core.Eventing;

/// <summary>
/// Defines a handler for a specific event type.
/// </summary>
/// <typeparam name="TEvent">The type of event to handle.</typeparam>
public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    /// <summary>
    /// Handles the specified event.
    /// </summary>
    /// <param name="event">The event to handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
