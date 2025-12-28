using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eQuantic.Core.Eventing.Kafka;

/// <summary>
/// Apache Kafka implementation of IExternalEventSubscriber.
/// </summary>
public class KafkaEventSubscriber : IExternalEventSubscriber
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IEventDispatcher _dispatcher;
    private readonly KafkaEventSubscriberOptions _options;
    private readonly ILogger<KafkaEventSubscriber>? _logger;
    private CancellationTokenSource? _cts;
    private Task? _consumeTask;

    public KafkaEventSubscriber(
        IConsumer<string, string> consumer,
        IEventDispatcher dispatcher,
        IOptions<KafkaEventSubscriberOptions> options,
        ILogger<KafkaEventSubscriber>? logger = null)
    {
        _consumer = consumer;
        _dispatcher = dispatcher;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        _consumer.Subscribe(_options.Topics);
        
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _consumeTask = ConsumeLoopAsync(_cts.Token);
        
        _logger?.LogInformation("Started consuming from Kafka topics: {Topics}", string.Join(", ", _options.Topics));
        
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_cts != null)
        {
            _cts.Cancel();
            
            if (_consumeTask != null)
            {
                try
                {
                    await _consumeTask;
                }
                catch (OperationCanceledException)
                {
                    // Expected
                }
            }
            
            _consumer.Close();
            _cts.Dispose();
            _logger?.LogInformation("Stopped consuming from Kafka");
        }
    }

    private async Task ConsumeLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(1));
                
                if (consumeResult == null)
                    continue;

                await ProcessMessageAsync(consumeResult, cancellationToken);
                
                if (_options.EnableAutoCommit == false)
                {
                    _consumer.Commit(consumeResult);
                }
            }
            catch (ConsumeException ex)
            {
                _logger?.LogError(ex, "Error consuming from Kafka: {Reason}", ex.Error.Reason);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error processing Kafka message");
            }
        }
    }

    private async Task ProcessMessageAsync(ConsumeResult<string, string> consumeResult, CancellationToken cancellationToken)
    {
        var eventType = ResolveEventType(consumeResult);
        
        if (eventType == null)
        {
            _logger?.LogWarning("Could not resolve event type for message at offset {Offset}", 
                consumeResult.Offset.Value);
            return;
        }

        var @event = JsonSerializer.Deserialize(
            consumeResult.Message.Value, 
            eventType, 
            _options.JsonSerializerOptions) as IEvent;

        if (@event != null)
        {
            await _dispatcher.DispatchAsync(@event, cancellationToken);
            _logger?.LogDebug("Dispatched event {EventType} from Kafka offset {Offset}", 
                eventType.Name, consumeResult.Offset.Value);
        }
    }

    private Type? ResolveEventType(ConsumeResult<string, string> consumeResult)
    {
        // Try to get from headers first
        var eventTypeHeader = consumeResult.Message.Headers.FirstOrDefault(h => h.Key == "EventType");
        
        if (eventTypeHeader != null)
        {
            var typeName = Encoding.UTF8.GetString(eventTypeHeader.GetValueBytes());
            
            if (_options.EventTypeResolver != null)
            {
                return _options.EventTypeResolver(typeName);
            }
            
            return Type.GetType(typeName);
        }

        return null;
    }

    public ValueTask DisposeAsync()
    {
        _consumer.Dispose();
        _cts?.Dispose();
        return ValueTask.CompletedTask;
    }
}
