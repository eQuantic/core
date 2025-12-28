using System.Text.Json;

namespace eQuantic.Core.Eventing.Azure;

/// <summary>
/// Configuration options for Azure Service Bus event subscriber.
/// </summary>
public class ServiceBusEventSubscriberOptions : ExternalEventSubscriberOptions
{
    /// <summary>
    /// Gets or sets the Azure Service Bus connection string.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the JSON serializer options.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
