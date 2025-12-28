using System.Text.Json;

namespace eQuantic.Core.Eventing.RabbitMQ;

/// <summary>
/// Configuration options for RabbitMQ event subscriber.
/// </summary>
public class RabbitMqEventSubscriberOptions : ExternalEventSubscriberOptions
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
    /// Gets or sets the queue name to consume from.
    /// </summary>
    public string? QueueName { get; set; }

    /// <summary>
    /// Gets or sets the exchange name.
    /// </summary>
    public string? ExchangeName { get; set; }

    /// <summary>
    /// Gets or sets the exchange type (direct, fanout, topic, headers).
    /// </summary>
    public string ExchangeType { get; set; } = "topic";

    /// <summary>
    /// Gets or sets the routing key pattern for binding.
    /// </summary>
    public string? RoutingKey { get; set; }

    /// <summary>
    /// Gets or sets whether the queue/exchange is durable.
    /// </summary>
    public bool Durable { get; set; } = true;

    /// <summary>
    /// Gets or sets the JSON serializer options.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
