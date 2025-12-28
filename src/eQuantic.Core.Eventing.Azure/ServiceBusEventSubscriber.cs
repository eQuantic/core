using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eQuantic.Core.Eventing.Azure;

/// <summary>
/// Azure Service Bus implementation of IExternalEventSubscriber.
/// </summary>
public class ServiceBusEventSubscriber : IExternalEventSubscriber
{
    private readonly ServiceBusClient _client;
    private readonly IEventDispatcher _dispatcher;
    private readonly ServiceBusEventSubscriberOptions _options;
    private readonly ILogger<ServiceBusEventSubscriber>? _logger;
    private ServiceBusProcessor? _processor;

    public ServiceBusEventSubscriber(
        ServiceBusClient client,
        IEventDispatcher dispatcher,
        IOptions<ServiceBusEventSubscriberOptions> options,
        ILogger<ServiceBusEventSubscriber>? logger = null)
    {
        _client = client;
        _dispatcher = dispatcher;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _processor = string.IsNullOrEmpty(_options.SubscriptionName)
            ? _client.CreateProcessor(_options.TopicName, new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = _options.MaxConcurrentCalls,
                AutoCompleteMessages = _options.AutoComplete
            })
            : _client.CreateProcessor(_options.TopicName, _options.SubscriptionName, new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = _options.MaxConcurrentCalls,
                AutoCompleteMessages = _options.AutoComplete
            });

        _processor.ProcessMessageAsync += ProcessMessageAsync;
        _processor.ProcessErrorAsync += ProcessErrorAsync;

        await _processor.StartProcessingAsync(cancellationToken);
        
        _logger?.LogInformation("Started listening to {Topic}/{Subscription}", 
            _options.TopicName, _options.SubscriptionName ?? "(queue)");
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_processor != null)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            _logger?.LogInformation("Stopped listening to {Topic}", _options.TopicName);
        }
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        try
        {
            var eventType = ResolveEventType(args);
            
            if (eventType == null)
            {
                _logger?.LogWarning("Could not resolve event type for message {MessageId}", args.Message.MessageId);
                return;
            }

            var @event = JsonSerializer.Deserialize(
                args.Message.Body.ToString(), 
                eventType, 
                _options.JsonSerializerOptions) as IEvent;

            if (@event != null)
            {
                await _dispatcher.DispatchAsync(@event, args.CancellationToken);
                _logger?.LogDebug("Dispatched event {EventType} from message {MessageId}", 
                    eventType.Name, args.Message.MessageId);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error processing message {MessageId}", args.Message.MessageId);
            throw;
        }
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        _logger?.LogError(args.Exception, "Error in Service Bus processor: {ErrorSource}", args.ErrorSource);
        return Task.CompletedTask;
    }

    private Type? ResolveEventType(ProcessMessageEventArgs args)
    {
        if (_options.EventTypeResolver != null)
        {
            if (args.Message.ApplicationProperties.TryGetValue("EventType", out var eventTypeName))
            {
                return _options.EventTypeResolver(eventTypeName?.ToString() ?? string.Empty);
            }
        }

        // Try to resolve from message properties
        if (args.Message.ApplicationProperties.TryGetValue("EventType", out var typeName))
        {
            return Type.GetType(typeName?.ToString() ?? string.Empty);
        }

        return null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_processor != null)
        {
            await _processor.DisposeAsync();
        }
    }
}
