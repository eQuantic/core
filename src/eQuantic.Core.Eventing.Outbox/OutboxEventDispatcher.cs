using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace eQuantic.Core.Eventing.Outbox;

/// <summary>
/// Event dispatcher that stores events in an outbox for reliable delivery.
/// </summary>
public class OutboxEventDispatcher : IEventDispatcher
{
    private readonly IOutboxStore _outboxStore;
    private readonly ILogger<OutboxEventDispatcher>? _logger;
    private readonly OutboxOptions _options;

    public OutboxEventDispatcher(
        IOutboxStore outboxStore,
        Microsoft.Extensions.Options.IOptions<OutboxOptions> options,
        ILogger<OutboxEventDispatcher>? logger = null)
    {
        _outboxStore = outboxStore;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task DispatchAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        var message = CreateOutboxMessage(@event);
        await _outboxStore.AddAsync(message, cancellationToken);
        
        _logger?.LogDebug("Event {EventType} with ID {EventId} stored in outbox",
            @event.GetType().Name, @event.EventId);
    }

    /// <inheritdoc />
    public async Task DispatchAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
    {
        var messages = events.Select(CreateOutboxMessage).ToList();
        
        if (messages.Count == 0)
            return;

        await _outboxStore.AddRangeAsync(messages, cancellationToken);
        
        _logger?.LogDebug("{Count} events stored in outbox", messages.Count);
    }

    private OutboxMessage CreateOutboxMessage<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var payload = JsonSerializer.Serialize(@event, _options.JsonSerializerOptions);
        
        return new OutboxMessage
        {
            EventId = @event.EventId,
            EventType = @event.GetType().AssemblyQualifiedName ?? @event.GetType().FullName!,
            Payload = payload,
            OccurredAt = @event.OccurredAt
        };
    }
}
