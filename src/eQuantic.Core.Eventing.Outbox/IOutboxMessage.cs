namespace eQuantic.Core.Eventing.Outbox;

/// <summary>
/// Represents an outbox message for storing events before publishing.
/// </summary>
public interface IOutboxMessage
{
    /// <summary>
    /// Gets the unique identifier of the outbox message.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the event identifier.
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// Gets the event type name.
    /// </summary>
    string EventType { get; }

    /// <summary>
    /// Gets the serialized event payload.
    /// </summary>
    string Payload { get; }

    /// <summary>
    /// Gets when the event occurred.
    /// </summary>
    DateTimeOffset OccurredAt { get; }

    /// <summary>
    /// Gets when the message was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Gets when the message was processed (null if not processed).
    /// </summary>
    DateTimeOffset? ProcessedAt { get; }

    /// <summary>
    /// Gets the number of processing attempts.
    /// </summary>
    int Attempts { get; }

    /// <summary>
    /// Gets the last error message if processing failed.
    /// </summary>
    string? LastError { get; }
}

/// <summary>
/// Default implementation of IOutboxMessage.
/// </summary>
public class OutboxMessage : IOutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTimeOffset OccurredAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ProcessedAt { get; set; }
    public int Attempts { get; set; }
    public string? LastError { get; set; }
}
