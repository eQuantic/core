namespace eQuantic.Core.Eventing;

/// <summary>
/// Interface for publishing events to external message brokers (Azure Service Bus, AWS SNS, RabbitMQ, etc.).
/// </summary>
public interface IExternalEventPublisher
{
    /// <summary>
    /// Publishes a single event to an external message broker.
    /// </summary>
    /// <typeparam name="TEvent">The type of event.</typeparam>
    /// <param name="event">The event to publish.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent;

    /// <summary>
    /// Publishes multiple events to an external message broker.
    /// </summary>
    /// <param name="events">The events to publish.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task PublishAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken = default);
}

/// <summary>
/// Options for external event publishing.
/// </summary>
public class ExternalEventPublisherOptions
{
    /// <summary>
    /// Gets or sets the topic/queue name for publishing events.
    /// </summary>
    public string? TopicName { get; set; }

    /// <summary>
    /// Gets or sets whether to include event type as a message property.
    /// </summary>
    public bool IncludeEventType { get; set; } = true;

    /// <summary>
    /// Gets or sets the serializer type to use.
    /// </summary>
    public EventSerializerType SerializerType { get; set; } = EventSerializerType.SystemTextJson;
}

/// <summary>
/// Event serializer types.
/// </summary>
public enum EventSerializerType
{
    /// <summary>
    /// Use System.Text.Json for serialization.
    /// </summary>
    SystemTextJson,

    /// <summary>
    /// Use Newtonsoft.Json for serialization.
    /// </summary>
    NewtonsoftJson
}
