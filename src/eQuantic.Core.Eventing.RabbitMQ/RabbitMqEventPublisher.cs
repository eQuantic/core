using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace eQuantic.Core.Eventing.RabbitMQ;

/// <summary>
/// RabbitMQ implementation of IExternalEventPublisher.
/// </summary>
public class RabbitMqEventPublisher : IExternalEventPublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly RabbitMqEventPublisherOptions _options;

    public RabbitMqEventPublisher(
        IConnection connection,
        IOptions<RabbitMqEventPublisherOptions> options)
    {
        _connection = connection;
        _options = options.Value;
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        
        // Declare exchange if configured
        if (!string.IsNullOrEmpty(_options.ExchangeName))
        {
            _channel.ExchangeDeclareAsync(
                exchange: _options.ExchangeName,
                type: _options.ExchangeType,
                durable: _options.Durable,
                autoDelete: false).GetAwaiter().GetResult();
        }
    }

    /// <inheritdoc />
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        var (body, properties) = await CreateMessageAsync(@event);
        var routingKey = GetRoutingKey(@event);
        
        await _channel.BasicPublishAsync(
            exchange: _options.ExchangeName ?? string.Empty,
            routingKey: routingKey,
            mandatory: _options.Mandatory,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task PublishAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var @event in events)
        {
            await PublishSingleEventAsync(@event, cancellationToken);
        }
    }

    private async Task PublishSingleEventAsync(IEvent @event, CancellationToken cancellationToken)
    {
        var (body, properties) = await CreateMessageAsync(@event);
        var routingKey = GetRoutingKey(@event);
        
        await _channel.BasicPublishAsync(
            exchange: _options.ExchangeName ?? string.Empty,
            routingKey: routingKey,
            mandatory: _options.Mandatory,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }

    private Task<(ReadOnlyMemory<byte> Body, BasicProperties Properties)> CreateMessageAsync<TEvent>(TEvent @event) 
        where TEvent : IEvent
    {
        var json = JsonSerializer.Serialize(@event, _options.JsonSerializerOptions);
        var body = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(json));
        
        var properties = new BasicProperties
        {
            ContentType = "application/json",
            MessageId = @event.EventId.ToString(),
            Persistent = _options.Persistent,
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        };

        if (_options.IncludeEventType)
        {
            properties.Headers ??= new Dictionary<string, object?>();
            properties.Headers["EventType"] = @event.GetType().FullName;
            properties.Headers["EventId"] = @event.EventId.ToString();
            properties.Headers["OccurredAt"] = @event.OccurredAt.ToString("O");
        }

        return Task.FromResult((body, properties));
    }

    private string GetRoutingKey<TEvent>(TEvent @event) where TEvent : IEvent
    {
        if (!string.IsNullOrEmpty(_options.RoutingKey))
            return _options.RoutingKey;

        if (_options.UseEventTypeAsRoutingKey)
            return @event.GetType().Name;

        return string.Empty;
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}
