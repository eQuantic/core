namespace eQuantic.Core.Eventing;

/// <summary>
/// Marker interface for all events in the eQuantic ecosystem.
/// This is the base interface for both Domain Events (DDD) and Notifications (CQS).
/// </summary>
public interface IEvent
{
    /// <summary>
    /// Gets the unique identifier of the event instance.
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// Gets the timestamp when the event occurred.
    /// </summary>
    DateTimeOffset OccurredAt { get; }
}

/// <summary>
/// Base implementation of IEvent with default values.
/// </summary>
public abstract class EventBase : IEvent
{
    /// <inheritdoc />
    public Guid EventId { get; protected set; } = Guid.NewGuid();

    /// <inheritdoc />
    public DateTimeOffset OccurredAt { get; protected set; } = DateTimeOffset.UtcNow;

    protected EventBase()
    {
    }

    protected EventBase(Guid eventId, DateTimeOffset occurredAt)
    {
        EventId = eventId;
        OccurredAt = occurredAt;
    }
}
