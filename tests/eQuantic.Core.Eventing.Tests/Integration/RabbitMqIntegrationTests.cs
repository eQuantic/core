using eQuantic.Core.Eventing.RabbitMQ;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Testcontainers.RabbitMq;
using Xunit;

namespace eQuantic.Core.Eventing.Tests.Integration;

// ============================================================
// TEST EVENT
// ============================================================

public class OrderCreatedEvent : EventBase
{
    public Guid OrderId { get; }
    public decimal Amount { get; }

    public OrderCreatedEvent(Guid orderId, decimal amount)
    {
        OrderId = orderId;
        Amount = amount;
    }
}

// ============================================================
// RABBITMQ INTEGRATION TESTS
// ============================================================

public class RabbitMqIntegrationTests : IAsyncLifetime
{
    private readonly RabbitMqContainer _container;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqIntegrationTests()
    {
        // Use explicit username/password
        _container = new RabbitMqBuilder()
            .WithImage("rabbitmq:3.13-management")
            .WithUsername("testuser")
            .WithPassword("testpass")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        
        // Wait for RabbitMQ to be fully ready
        await Task.Delay(2000);
        
        // Create a connection with retry
        var factory = new ConnectionFactory
        {
            HostName = _container.Hostname,
            Port = _container.GetMappedPublicPort(5672),
            UserName = "testuser",
            Password = "testpass"
        };
        
        // Retry connection up to 5 times
        for (int i = 0; i < 5; i++)
        {
            try
            {
                _connection = await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
                break;
            }
            catch
            {
                if (i == 4) throw;
                await Task.Delay(2000);
            }
        }
    }

    public async Task DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
        }
        if (_connection != null)
        {
            await _connection.CloseAsync();
        }
        await _container.DisposeAsync();
    }

    private RabbitMqEventPublisher CreatePublisher(string exchangeName, string exchangeType = "topic")
    {
        var options = Options.Create(new RabbitMqEventPublisherOptions
        {
            HostName = _container.Hostname,
            Port = _container.GetMappedPublicPort(5672),
            UserName = "testuser",
            Password = "testpass",
            ExchangeName = exchangeName,
            ExchangeType = exchangeType,
            UseEventTypeAsRoutingKey = true
        });

        return new RabbitMqEventPublisher(_connection!, options);
    }

    [Fact]
    public async Task RabbitMqPublisher_ShouldPublishEventToExchange()
    {
        // Arrange
        var publisher = CreatePublisher("test-events");
        var @event = new OrderCreatedEvent(Guid.NewGuid(), 99.99m);

        // Act
        var act = () => publisher.PublishAsync(@event);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task RabbitMqPublisher_ShouldPublishMultipleEvents()
    {
        // Arrange
        var publisher = CreatePublisher("test-batch-events", "fanout");
        var events = new List<IEvent>
        {
            new OrderCreatedEvent(Guid.NewGuid(), 100m),
            new OrderCreatedEvent(Guid.NewGuid(), 200m),
            new OrderCreatedEvent(Guid.NewGuid(), 300m)
        };

        // Act
        var act = () => publisher.PublishAsync(events);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task RabbitMqPublisher_WithEmptyEvents_ShouldNotThrow()
    {
        // Arrange
        var publisher = CreatePublisher("test-empty-events");

        // Act
        var act = () => publisher.PublishAsync(new List<IEvent>());

        // Assert
        await act.Should().NotThrowAsync();
    }
}

// ============================================================
// DI REGISTRATION TESTS
// ============================================================

public class RabbitMqServiceCollectionTests
{
    [Fact]
    public void AddRabbitMqPublisher_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddRabbitMqPublisher(options =>
        {
            options.HostName = "localhost";
            options.ExchangeName = "test";
        });

        // Assert
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IExternalEventPublisher));
        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddRabbitMqPublisher_ShouldConfigureOptions()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddRabbitMqPublisher(options =>
        {
            options.HostName = "my-host";
            options.Port = 5673;
            options.ExchangeName = "my-exchange";
            options.ExchangeType = "fanout";
        });

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<RabbitMqEventPublisherOptions>>().Value;

        // Assert
        options.HostName.Should().Be("my-host");
        options.Port.Should().Be(5673);
        options.ExchangeName.Should().Be("my-exchange");
        options.ExchangeType.Should().Be("fanout");
    }
}
