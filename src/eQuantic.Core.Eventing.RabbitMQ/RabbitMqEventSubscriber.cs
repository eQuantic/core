using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace eQuantic.Core.Eventing.RabbitMQ;

/// <summary>
/// RabbitMQ implementation of IExternalEventSubscriber.
/// </summary>
public class RabbitMqEventSubscriber : IExternalEventSubscriber
{
    private readonly IConnection _connection;
    private readonly IEventDispatcher _dispatcher;
    private readonly RabbitMqEventSubscriberOptions _options;
    private readonly ILogger<RabbitMqEventSubscriber>? _logger;
    private IChannel? _channel;
    private string? _consumerTag;

    public RabbitMqEventSubscriber(
        IConnection connection,
        IEventDispatcher dispatcher,
        IOptions<RabbitMqEventSubscriberOptions> options,
        ILogger<RabbitMqEventSubscriber>? logger = null)
    {
        _connection = connection;
        _dispatcher = dispatcher;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        // Declare exchange if configured
        if (!string.IsNullOrEmpty(_options.ExchangeName))
        {
            await _channel.ExchangeDeclareAsync(
                exchange: _options.ExchangeName,
                type: _options.ExchangeType,
                durable: _options.Durable,
                autoDelete: false,
                cancellationToken: cancellationToken);
        }

        // Declare queue
        await _channel.QueueDeclareAsync(
            queue: _options.QueueName,
            durable: _options.Durable,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        // Bind queue to exchange
        if (!string.IsNullOrEmpty(_options.ExchangeName))
        {
            var routingKey = _options.RoutingKey ?? "#";
            await _channel.QueueBindAsync(
                queue: _options.QueueName!,
                exchange: _options.ExchangeName,
                routingKey: routingKey,
                cancellationToken: cancellationToken);
        }

        // Set prefetch
        await _channel.BasicQosAsync(0, (ushort)_options.MaxConcurrentCalls, false, cancellationToken);

        // Create consumer
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += ProcessMessageAsync;

        _consumerTag = await _channel.BasicConsumeAsync(
            queue: _options.QueueName!,
            autoAck: _options.AutoComplete,
            consumer: consumer,
            cancellationToken: cancellationToken);

        _logger?.LogInformation("Started listening to queue {Queue}", _options.QueueName);
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_channel != null && _consumerTag != null)
        {
            await _channel.BasicCancelAsync(_consumerTag, cancellationToken: cancellationToken);
            _logger?.LogInformation("Stopped listening to queue {Queue}", _options.QueueName);
        }
    }

    private async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs args)
    {
        try
        {
            var eventType = ResolveEventType(args);
            
            if (eventType == null)
            {
                _logger?.LogWarning("Could not resolve event type for message {DeliveryTag}", args.DeliveryTag);
                return;
            }

            var body = Encoding.UTF8.GetString(args.Body.ToArray());
            var @event = JsonSerializer.Deserialize(body, eventType, _options.JsonSerializerOptions) as IEvent;

            if (@event != null)
            {
                await _dispatcher.DispatchAsync(@event);
                _logger?.LogDebug("Dispatched event {EventType} from message {DeliveryTag}", 
                    eventType.Name, args.DeliveryTag);
            }

            // Manual ack if not auto-complete
            if (!_options.AutoComplete && _channel != null)
            {
                await _channel.BasicAckAsync(args.DeliveryTag, false);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error processing message {DeliveryTag}", args.DeliveryTag);
            
            // Nack and requeue on error
            if (!_options.AutoComplete && _channel != null)
            {
                await _channel.BasicNackAsync(args.DeliveryTag, false, true);
            }
        }
    }

    private Type? ResolveEventType(BasicDeliverEventArgs args)
    {
        if (_options.EventTypeResolver != null)
        {
            if (args.BasicProperties.Headers?.TryGetValue("EventType", out var eventTypeName) == true)
            {
                var typeName = eventTypeName is byte[] bytes 
                    ? Encoding.UTF8.GetString(bytes) 
                    : eventTypeName?.ToString() ?? string.Empty;
                return _options.EventTypeResolver(typeName);
            }
        }

        // Try to resolve from headers
        if (args.BasicProperties.Headers?.TryGetValue("EventType", out var typeName2) == true)
        {
            var name = typeName2 is byte[] bytes 
                ? Encoding.UTF8.GetString(bytes) 
                : typeName2?.ToString() ?? string.Empty;
            return Type.GetType(name);
        }

        return null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
        }
    }
}
