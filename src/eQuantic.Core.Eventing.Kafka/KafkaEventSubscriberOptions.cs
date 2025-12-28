using System.Text.Json;
using Confluent.Kafka;

namespace eQuantic.Core.Eventing.Kafka;

/// <summary>
/// Configuration options for Kafka event subscriber.
/// </summary>
public class KafkaEventSubscriberOptions : ExternalEventSubscriberOptions
{
    /// <summary>
    /// Gets or sets the Kafka bootstrap servers (comma-separated).
    /// </summary>
    public string BootstrapServers { get; set; } = "localhost:9092";

    /// <summary>
    /// Gets or sets the topics to subscribe to.
    /// </summary>
    public List<string> Topics { get; set; } = new();

    /// <summary>
    /// Gets or sets the consumer group ID.
    /// </summary>
    public string GroupId { get; set; } = "default-consumer-group";

    /// <summary>
    /// Gets or sets the auto offset reset behavior.
    /// </summary>
    public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Earliest;

    /// <summary>
    /// Gets or sets whether to enable auto commit.
    /// </summary>
    public bool EnableAutoCommit { get; set; } = false;

    /// <summary>
    /// Gets or sets the consumer configuration.
    /// </summary>
    public ConsumerConfig? ConsumerConfig { get; set; }

    /// <summary>
    /// Gets or sets the JSON serializer options.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
