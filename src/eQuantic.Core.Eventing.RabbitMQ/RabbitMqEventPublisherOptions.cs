using System.Text.Json;

namespace eQuantic.Core.Eventing.RabbitMQ;

/// <summary>
/// Configuration options for RabbitMQ event publisher.
/// </summary>
public class RabbitMqEventPublisherOptions : ExternalEventPublisherOptions
{
    /// <summary>
    /// Gets or sets the RabbitMQ host name.
    /// </summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>
    /// Gets or sets the RabbitMQ port.
    /// </summary>
    public int Port { get; set; } = 5672;

    /// <summary>
    /// Gets or sets the RabbitMQ username.
    /// </summary>
    public string UserName { get; set; } = "guest";

    /// <summary>
    /// Gets or sets the RabbitMQ password.
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// Gets or sets the virtual host.
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Gets or sets the exchange name.
    /// </summary>
    public string? ExchangeName { get; set; }

    /// <summary>
    /// Gets or sets the exchange type (direct, fanout, topic, headers).
    /// </summary>
    public string ExchangeType { get; set; } = "topic";

    /// <summary>
    /// Gets or sets the routing key.
    /// </summary>
    public string? RoutingKey { get; set; }

    /// <summary>
    /// Gets or sets whether to use event type name as routing key.
    /// </summary>
    public bool UseEventTypeAsRoutingKey { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the exchange is durable.
    /// </summary>
    public bool Durable { get; set; } = true;

    /// <summary>
    /// Gets or sets whether messages are persistent.
    /// </summary>
    public bool Persistent { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to use mandatory publishing.
    /// </summary>
    public bool Mandatory { get; set; }

    /// <summary>
    /// Gets or sets the JSON serializer options.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };
}
