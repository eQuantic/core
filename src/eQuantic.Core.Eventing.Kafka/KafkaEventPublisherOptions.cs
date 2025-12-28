using System.Text.Json;
using Confluent.Kafka;

namespace eQuantic.Core.Eventing.Kafka;

/// <summary>
/// Configuration options for Kafka event publisher.
/// </summary>
public class KafkaEventPublisherOptions : ExternalEventPublisherOptions
{
    /// <summary>
    /// Gets or sets the Kafka bootstrap servers (comma-separated).
    /// </summary>
    public string BootstrapServers { get; set; } = "localhost:9092";

    /// <summary>
    /// Gets or sets optional custom key selector for message partitioning.
    /// If not set, EventId will be used as the key.
    /// </summary>
    public Func<IEvent, string>? KeySelector { get; set; }

    /// <summary>
    /// Gets or sets the producer configuration.
    /// </summary>
    public ProducerConfig? ProducerConfig { get; set; }

    /// <summary>
    /// Gets or sets the delivery guarantee mode.
    /// </summary>
    public Acks Acks { get; set; } = Acks.All;

    /// <summary>
    /// Gets or sets the JSON serializer options.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
