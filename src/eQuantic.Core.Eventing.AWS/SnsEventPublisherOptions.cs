using System.Text.Json;

namespace eQuantic.Core.Eventing.AWS;

/// <summary>
/// Configuration options for AWS SNS event publisher.
/// </summary>
public class SnsEventPublisherOptions : ExternalEventPublisherOptions
{
    /// <summary>
    /// Gets or sets the AWS SNS Topic ARN.
    /// </summary>
    public string? TopicArn { get; set; }

    /// <summary>
    /// Gets or sets the AWS region.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// Gets or sets the message group ID for FIFO topics.
    /// </summary>
    public string? MessageGroupId { get; set; }

    /// <summary>
    /// Gets or sets whether to use EventId as deduplication ID for FIFO topics.
    /// </summary>
    public bool UseFifoDeduplication { get; set; }

    /// <summary>
    /// Gets or sets the JSON serializer options.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };
}
