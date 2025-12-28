using System.Text.Json;

namespace eQuantic.Core.Eventing.AWS;

/// <summary>
/// Configuration options for AWS SQS event subscriber.
/// </summary>
public class SqsEventSubscriberOptions : ExternalEventSubscriberOptions
{
    /// <summary>
    /// Gets or sets the SQS queue URL.
    /// </summary>
    public string? QueueUrl { get; set; }

    /// <summary>
    /// Gets or sets the AWS region.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// Gets or sets the wait time in seconds for long polling.
    /// </summary>
    public int WaitTimeSeconds { get; set; } = 20;

    /// <summary>
    /// Gets or sets whether to unwrap SNS notification wrapper.
    /// Set to true if messages come from SNS → SQS.
    /// </summary>
    public bool UnwrapSnsMessage { get; set; } = true;

    /// <summary>
    /// Gets or sets the JSON serializer options.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
