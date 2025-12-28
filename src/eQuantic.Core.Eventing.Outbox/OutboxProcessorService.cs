using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eQuantic.Core.Eventing.Outbox;

/// <summary>
/// Background service that processes outbox messages.
/// </summary>
public class OutboxProcessorService : BackgroundService
{
    private readonly IOutboxStore _outboxStore;
    private readonly IExternalEventPublisher _publisher;
    private readonly OutboxOptions _options;
    private readonly ILogger<OutboxProcessorService> _logger;

    public OutboxProcessorService(
        IOutboxStore outboxStore,
        IExternalEventPublisher publisher,
        Microsoft.Extensions.Options.IOptions<OutboxOptions> options,
        ILogger<OutboxProcessorService> logger)
    {
        _outboxStore = outboxStore;
        _publisher = publisher;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox processor started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxAsync(stoppingToken);
                await CleanupProcessedAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing outbox");
            }

            await Task.Delay(_options.ProcessingInterval, stoppingToken);
        }

        _logger.LogInformation("Outbox processor stopped");
    }

    private async Task ProcessOutboxAsync(CancellationToken cancellationToken)
    {
        var messages = await _outboxStore.GetPendingAsync(_options.BatchSize, cancellationToken);

        if (messages.Count == 0)
            return;

        _logger.LogDebug("Processing {Count} outbox messages", messages.Count);

        foreach (var message in messages)
        {
            if (message.Attempts >= _options.MaxRetryAttempts)
            {
                _logger.LogWarning("Message {Id} exceeded max retry attempts", message.Id);
                continue;
            }

            try
            {
                var @event = DeserializeEvent(message);
                
                if (@event != null)
                {
                    await _publisher.PublishAsync(@event, cancellationToken);
                    await _outboxStore.MarkAsProcessedAsync(message.Id, cancellationToken);
                    
                    _logger.LogDebug("Published message {Id}", message.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process message {Id}", message.Id);
                await _outboxStore.MarkAsFailedAsync(message.Id, ex.Message, cancellationToken);
            }
        }
    }

    private async Task CleanupProcessedAsync(CancellationToken cancellationToken)
    {
        var threshold = DateTimeOffset.UtcNow.Subtract(_options.RetentionPeriod);
        await _outboxStore.DeleteProcessedAsync(threshold, cancellationToken);
    }

    private IEvent? DeserializeEvent(IOutboxMessage message)
    {
        var eventType = Type.GetType(message.EventType);
        
        if (eventType == null)
        {
            _logger.LogWarning("Could not resolve event type: {EventType}", message.EventType);
            return null;
        }

        return JsonSerializer.Deserialize(message.Payload, eventType, _options.JsonSerializerOptions) as IEvent;
    }
}
