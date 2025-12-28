using Confluent.Kafka;
using eQuantic.Core.Eventing.Kafka;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Testcontainers.Kafka;
using Xunit;

namespace eQuantic.Core.Eventing.Tests.Integration;

// ============================================================
// KAFKA TEST EVENT
// ============================================================

public class UserCreatedEvent : EventBase
{
    public Guid UserId { get; }
    public string Email { get; }

    public UserCreatedEvent(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }
}

// ============================================================
// KAFKA INTEGRATION TESTS
// ============================================================

public class KafkaIntegrationTests : IAsyncLifetime
{
    private readonly KafkaContainer _container;

    public KafkaIntegrationTests()
    {
        _container = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:7.5.0")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    private KafkaEventPublisher CreatePublisher(string topicName)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _container.GetBootstrapAddress()
        };

        var producer = new ProducerBuilder<string, string>(config).Build();
        var options = Options.Create(new KafkaEventPublisherOptions
        {
            BootstrapServers = _container.GetBootstrapAddress(),
            TopicName = topicName,
            IncludeEventType = true
        });

        return new KafkaEventPublisher(producer, options);
    }

    [Fact]
    public async Task KafkaPublisher_ShouldPublishEventToTopic()
    {
        // Arrange
        var publisher = CreatePublisher("test-events");
        var @event = new UserCreatedEvent(Guid.NewGuid(), "test@example.com");

        // Act
        var act = () => publisher.PublishAsync(@event);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task KafkaPublisher_ShouldPublishMultipleEvents()
    {
        // Arrange
        var publisher = CreatePublisher("test-batch-events");
        var events = new List<IEvent>
        {
            new UserCreatedEvent(Guid.NewGuid(), "user1@example.com"),
            new UserCreatedEvent(Guid.NewGuid(), "user2@example.com"),
            new UserCreatedEvent(Guid.NewGuid(), "user3@example.com")
        };

        // Act
        var act = () => publisher.PublishAsync(events);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task KafkaPublisher_WithEmptyEvents_ShouldNotThrow()
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

public class KafkaServiceCollectionTests
{
    [Fact]
    public void AddKafkaPublisher_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddKafkaPublisher(options =>
        {
            options.BootstrapServers = "localhost:9092";
            options.TopicName = "test";
        });

        // Assert
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IExternalEventPublisher));
        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddKafkaPublisher_ShouldConfigureOptions()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddKafkaPublisher(options =>
        {
            options.BootstrapServers = "my-kafka:9092";
            options.TopicName = "my-topic";
        });

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<KafkaEventPublisherOptions>>().Value;

        // Assert
        options.BootstrapServers.Should().Be("my-kafka:9092");
        options.TopicName.Should().Be("my-topic");
    }

    [Fact]
    public void AddKafkaSubscriber_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddKafkaSubscriber(options =>
        {
            options.BootstrapServers = "localhost:9092";
            options.Topics = new List<string> { "test" };
            options.GroupId = "test-group";
        });

        // Assert
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IExternalEventSubscriber));
        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }
}
