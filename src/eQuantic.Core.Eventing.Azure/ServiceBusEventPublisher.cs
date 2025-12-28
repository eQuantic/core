using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;

namespace eQuantic.Core.Eventing.Azure;

/// <summary>
/// Azure Service Bus implementation of IExternalEventPublisher.
/// </summary>
public class ServiceBusEventPublisher : IExternalEventPublisher, IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;
    private readonly ServiceBusEventPublisherOptions _options;

    public ServiceBusEventPublisher(
        ServiceBusClient client,
        IOptions<ServiceBusEventPublisherOptions> options)
    {
        _client = client;
        _options = options.Value;
        _sender = _client.CreateSender(_options.TopicName);
    }

    /// <inheritdoc />
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        var message = CreateMessage(@event);
        await _sender.SendMessageAsync(message, cancellationToken);
    }

    /// <inheritdoc />
    public async Task PublishAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
    {
        var messages = events.Select(CreateMessage).ToList();
        
        if (messages.Count == 0)
            return;

        if (messages.Count == 1)
        {
            await _sender.SendMessageAsync(messages[0], cancellationToken);
            return;
        }

        // Use batch for multiple messages
        using var batch = await _sender.CreateMessageBatchAsync(cancellationToken);
        
        foreach (var message in messages)
        {
            if (!batch.TryAddMessage(message))
            {
                // If batch is full, send it and create a new one
                await _sender.SendMessagesAsync(batch, cancellationToken);
                using var newBatch = await _sender.CreateMessageBatchAsync(cancellationToken);
                newBatch.TryAddMessage(message);
            }
        }

        if (batch.Count > 0)
        {
            await _sender.SendMessagesAsync(batch, cancellationToken);
        }
    }

    private ServiceBusMessage CreateMessage<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var json = JsonSerializer.Serialize(@event, _options.JsonSerializerOptions);
        var message = new ServiceBusMessage(json)
        {
            ContentType = "application/json",
            MessageId = @event.EventId.ToString()
        };

        if (_options.IncludeEventType)
        {
            message.ApplicationProperties["EventType"] = @event.GetType().FullName;
            message.ApplicationProperties["EventId"] = @event.EventId.ToString();
            message.ApplicationProperties["OccurredAt"] = @event.OccurredAt.ToString("O");
        }

        if (!string.IsNullOrEmpty(_options.SessionIdProperty))
        {
            var sessionId = GetPropertyValue(@event, _options.SessionIdProperty);
            if (!string.IsNullOrEmpty(sessionId))
            {
                message.SessionId = sessionId;
            }
        }

        return message;
    }

    private static string? GetPropertyValue<TEvent>(TEvent @event, string propertyName)
    {
        var property = typeof(TEvent).GetProperty(propertyName);
        return property?.GetValue(@event)?.ToString();
    }

    public async ValueTask DisposeAsync()
    {
        await _sender.DisposeAsync();
        await _client.DisposeAsync();
    }
}
