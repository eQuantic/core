namespace eQuantic.Core.Eventing;

/// <summary>
/// Interface for subscribing to events from external message brokers (Azure Service Bus, AWS SQS, RabbitMQ, etc.).
/// </summary>
public interface IExternalEventSubscriber : IAsyncDisposable
{
    /// <summary>
    /// Starts listening for events from the external message broker.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops listening for events.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task StopAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Options for external event subscription.
/// </summary>
public class ExternalEventSubscriberOptions
{
    /// <summary>
    /// Gets or sets the topic/queue name to subscribe to.
    /// </summary>
    public string? SubscriptionName { get; set; }

    /// <summary>
    /// Gets or sets the subscription name (for topics with multiple subscriptions).
    /// </summary>
    public string? TopicName { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of concurrent message handlers.
    /// </summary>
    public int MaxConcurrentCalls { get; set; } = 1;

    /// <summary>
    /// Gets or sets whether to auto-complete messages after successful processing.
    /// </summary>
    public bool AutoComplete { get; set; } = true;

    /// <summary>
    /// Gets or sets the event type resolver for deserializing events.
    /// </summary>
    public Func<string, Type?>? EventTypeResolver { get; set; }
}
