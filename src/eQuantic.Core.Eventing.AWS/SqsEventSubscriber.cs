using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eQuantic.Core.Eventing.AWS;

/// <summary>
/// AWS SQS implementation of IExternalEventSubscriber.
/// SNS publishes to SQS, and this subscriber consumes from SQS.
/// </summary>
public class SqsEventSubscriber : IExternalEventSubscriber
{
    private readonly IAmazonSQS _sqsClient;
    private readonly IEventDispatcher _dispatcher;
    private readonly SqsEventSubscriberOptions _options;
    private readonly ILogger<SqsEventSubscriber>? _logger;
    private CancellationTokenSource? _cts;
    private Task? _pollingTask;

    public SqsEventSubscriber(
        IAmazonSQS sqsClient,
        IEventDispatcher dispatcher,
        IOptions<SqsEventSubscriberOptions> options,
        ILogger<SqsEventSubscriber>? logger = null)
    {
        _sqsClient = sqsClient;
        _dispatcher = dispatcher;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _pollingTask = PollMessagesAsync(_cts.Token);
        
        _logger?.LogInformation("Started polling SQS queue {QueueUrl}", _options.QueueUrl);
        
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_cts != null)
        {
            _cts.Cancel();
            
            if (_pollingTask != null)
            {
                try
                {
                    await _pollingTask;
                }
                catch (OperationCanceledException)
                {
                    // Expected
                }
            }
            
            _cts.Dispose();
            _logger?.LogInformation("Stopped polling SQS queue {QueueUrl}", _options.QueueUrl);
        }
    }

    private async Task PollMessagesAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _options.QueueUrl,
                    MaxNumberOfMessages = _options.MaxConcurrentCalls,
                    WaitTimeSeconds = _options.WaitTimeSeconds,
                    MessageAttributeNames = new List<string> { "All" }
                };

                var response = await _sqsClient.ReceiveMessageAsync(request, cancellationToken);

                foreach (var message in response.Messages)
                {
                    await ProcessMessageAsync(message, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error polling SQS queue");
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }

    private async Task ProcessMessageAsync(Message message, CancellationToken cancellationToken)
    {
        try
        {
            var eventType = ResolveEventType(message);
            
            if (eventType == null)
            {
                _logger?.LogWarning("Could not resolve event type for message {MessageId}", message.MessageId);
                return;
            }

            // SNS wraps messages, so we may need to unwrap
            var body = _options.UnwrapSnsMessage ? UnwrapSnsMessage(message.Body) : message.Body;
            var @event = JsonSerializer.Deserialize(body, eventType, _options.JsonSerializerOptions) as IEvent;

            if (@event != null)
            {
                await _dispatcher.DispatchAsync(@event, cancellationToken);
                _logger?.LogDebug("Dispatched event {EventType} from message {MessageId}", 
                    eventType.Name, message.MessageId);
            }

            // Delete message after successful processing
            if (_options.AutoComplete)
            {
                await _sqsClient.DeleteMessageAsync(_options.QueueUrl, message.ReceiptHandle, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error processing message {MessageId}", message.MessageId);
        }
    }

    private Type? ResolveEventType(Message message)
    {
        if (_options.EventTypeResolver != null)
        {
            if (message.MessageAttributes.TryGetValue("EventType", out var attr))
            {
                return _options.EventTypeResolver(attr.StringValue ?? string.Empty);
            }
        }

        if (message.MessageAttributes.TryGetValue("EventType", out var typeAttr))
        {
            return Type.GetType(typeAttr.StringValue ?? string.Empty);
        }

        return null;
    }

    private string UnwrapSnsMessage(string body)
    {
        try
        {
            var snsMessage = JsonSerializer.Deserialize<SnsNotification>(body);
            return snsMessage?.Message ?? body;
        }
        catch
        {
            return body;
        }
    }

    public ValueTask DisposeAsync()
    {
        _cts?.Dispose();
        return ValueTask.CompletedTask;
    }

    private class SnsNotification
    {
        public string? Message { get; set; }
    }
}
