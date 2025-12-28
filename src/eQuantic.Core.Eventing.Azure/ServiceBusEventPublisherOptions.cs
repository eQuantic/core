using System.Text.Json;

namespace eQuantic.Core.Eventing.Azure;

/// <summary>
/// Configuration options for Azure Service Bus event publisher.
/// </summary>
public class ServiceBusEventPublisherOptions : ExternalEventPublisherOptions
{
    /// <summary>
    /// Gets or sets the Azure Service Bus connection string.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the property name to use as session ID (for session-enabled topics).
    /// </summary>
    public string? SessionIdProperty { get; set; }

    /// <summary>
    /// Gets or sets the JSON serializer options.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };
}
