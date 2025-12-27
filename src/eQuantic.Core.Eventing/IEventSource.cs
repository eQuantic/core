namespace eQuantic.Core.Eventing;

/// <summary>
/// Interface for entities that can collect and raise domain events.
/// Used by both Aggregate Roots (DDD) and other event-sourcing patterns.
/// </summary>
public interface IEventSource
{
    /// <summary>
    /// Gets the uncommitted events that have been raised by this entity.
    /// </summary>
    IReadOnlyCollection<IEvent> GetUncommittedEvents();

    /// <summary>
    /// Clears all uncommitted events after they have been dispatched/persisted.
    /// </summary>
    void ClearUncommittedEvents();
}

/// <summary>
/// Base class for entities that can raise events.
/// </summary>
public abstract class EventSourceBase : IEventSource
{
    private readonly List<IEvent> _uncommittedEvents = new();

    /// <summary>
    /// Adds an event to the uncommitted events collection.
    /// </summary>
    /// <param name="event">The event to add.</param>
    protected void AddEvent(IEvent @event)
    {
        _uncommittedEvents.Add(@event);
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IEvent> GetUncommittedEvents()
    {
        return _uncommittedEvents.AsReadOnly();
    }

    /// <inheritdoc />
    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }
}
