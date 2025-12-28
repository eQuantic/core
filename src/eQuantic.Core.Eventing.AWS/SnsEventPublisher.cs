using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;

namespace eQuantic.Core.Eventing.AWS;

/// <summary>
/// AWS SNS implementation of IExternalEventPublisher.
/// </summary>
public class SnsEventPublisher : IExternalEventPublisher
{
    private readonly IAmazonSimpleNotificationService _snsClient;
    private readonly SnsEventPublisherOptions _options;

    public SnsEventPublisher(
        IAmazonSimpleNotificationService snsClient,
        IOptions<SnsEventPublisherOptions> options)
    {
        _snsClient = snsClient;
        _options = options.Value;
    }

    /// <inheritdoc />
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        var request = CreatePublishRequest(@event);
        await _snsClient.PublishAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task PublishAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
    {
        var eventList = events.ToList();
        
        if (eventList.Count == 0)
            return;

        // SNS doesn't support batch publishing like SQS, so publish one by one
        // Consider using SQS with batch for high-throughput scenarios
        var tasks = eventList.Select(e => PublishSingleEventAsync(e, cancellationToken));
        await Task.WhenAll(tasks);
    }

    private async Task PublishSingleEventAsync(IEvent @event, CancellationToken cancellationToken)
    {
        var request = CreatePublishRequest(@event);
        await _snsClient.PublishAsync(request, cancellationToken);
    }

    private PublishRequest CreatePublishRequest<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var json = JsonSerializer.Serialize(@event, _options.JsonSerializerOptions);
        
        var request = new PublishRequest
        {
            TopicArn = _options.TopicArn,
            Message = json
        };

        if (_options.IncludeEventType)
        {
            request.MessageAttributes["EventType"] = new MessageAttributeValue
            {
                DataType = "String",
                StringValue = @event.GetType().FullName
            };

            request.MessageAttributes["EventId"] = new MessageAttributeValue
            {
                DataType = "String",
                StringValue = @event.EventId.ToString()
            };

            request.MessageAttributes["OccurredAt"] = new MessageAttributeValue
            {
                DataType = "String",
                StringValue = @event.OccurredAt.ToString("O")
            };
        }

        // Add message group ID for FIFO topics
        if (!string.IsNullOrEmpty(_options.MessageGroupId))
        {
            request.MessageGroupId = _options.MessageGroupId;
        }

        // Add deduplication ID for FIFO topics
        if (_options.UseFifoDeduplication)
        {
            request.MessageDeduplicationId = @event.EventId.ToString();
        }

        return request;
    }
}
