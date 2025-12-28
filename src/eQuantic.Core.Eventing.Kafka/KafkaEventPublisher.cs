using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eQuantic.Core.Eventing.Kafka;

/// <summary>
/// Apache Kafka implementation of IExternalEventPublisher.
/// </summary>
public class KafkaEventPublisher : IExternalEventPublisher, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly KafkaEventPublisherOptions _options;
    private readonly ILogger<KafkaEventPublisher>? _logger;

    public KafkaEventPublisher(
        IProducer<string, string> producer,
        IOptions<KafkaEventPublisherOptions> options,
        ILogger<KafkaEventPublisher>? logger = null)
    {
        _producer = producer;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : IEvent
    {
        var message = CreateMessage(@event);
        
        var result = await _producer.ProduceAsync(_options.TopicName, message, cancellationToken);
        
        _logger?.LogDebug("Published event {EventType} to Kafka topic {Topic} partition {Partition} offset {Offset}",
            typeof(TEvent).Name, result.Topic, result.Partition.Value, result.Offset.Value);
    }

    /// <inheritdoc />
    public async Task PublishAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var @event in events)
        {
            var message = CreateMessage(@event);
            await _producer.ProduceAsync(_options.TopicName, message, cancellationToken);
        }
        
        _producer.Flush(cancellationToken);
        _logger?.LogDebug("Published batch of events to Kafka topic {Topic}", _options.TopicName);
    }

    private Message<string, string> CreateMessage<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var eventType = @event.GetType();
        var payload = JsonSerializer.Serialize(@event, eventType, _options.JsonSerializerOptions);
        
        // Use EventId as key for partitioning
        var key = _options.KeySelector?.Invoke(@event) ?? @event.EventId.ToString();

        var headers = new Headers();
        
        if (_options.IncludeEventType)
        {
            headers.Add("EventType", Encoding.UTF8.GetBytes(eventType.FullName ?? eventType.Name));
        }
        
        headers.Add("EventId", Encoding.UTF8.GetBytes(@event.EventId.ToString()));
        headers.Add("OccurredAt", Encoding.UTF8.GetBytes(@event.OccurredAt.ToString("O")));

        return new Message<string, string>
        {
            Key = key,
            Value = payload,
            Headers = headers
        };
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}
