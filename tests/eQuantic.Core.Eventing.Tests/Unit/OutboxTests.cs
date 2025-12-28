using eQuantic.Core.Eventing.Outbox;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace eQuantic.Core.Eventing.Tests.Unit;

// ============================================================
// TEST EVENT
// ============================================================

public class TestEvent : EventBase
{
    public string Message { get; }

    public TestEvent(string message)
    {
        Message = message;
    }
}

// ============================================================
// OUTBOX DISPATCHER TESTS
// ============================================================

public class OutboxEventDispatcherTests
{
    private readonly Mock<IOutboxStore> _outboxStoreMock;
    private readonly OutboxEventDispatcher _dispatcher;

    public OutboxEventDispatcherTests()
    {
        _outboxStoreMock = new Mock<IOutboxStore>();
        var options = Options.Create(new OutboxOptions());
        _dispatcher = new OutboxEventDispatcher(_outboxStoreMock.Object, options);
    }

    [Fact]
    public async Task DispatchAsync_SingleEvent_ShouldStoreInOutbox()
    {
        // Arrange
        var @event = new TestEvent("Test message");

        // Act
        await _dispatcher.DispatchAsync(@event);

        // Assert
        _outboxStoreMock.Verify(
            x => x.AddAsync(It.Is<IOutboxMessage>(m => 
                m.EventType.Contains("TestEvent") &&
                m.EventId == @event.EventId), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task DispatchAsync_MultipleEvents_ShouldStoreAllInOutbox()
    {
        // Arrange
        var events = new List<IEvent>
        {
            new TestEvent("Message 1"),
            new TestEvent("Message 2"),
            new TestEvent("Message 3")
        };

        // Act
        await _dispatcher.DispatchAsync(events);

        // Assert
        _outboxStoreMock.Verify(
            x => x.AddRangeAsync(
                It.Is<IEnumerable<IOutboxMessage>>(m => m.Count() == 3),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DispatchAsync_EmptyEvents_ShouldNotCallStore()
    {
        // Arrange
        var events = new List<IEvent>();

        // Act
        await _dispatcher.DispatchAsync(events);

        // Assert
        _outboxStoreMock.Verify(
            x => x.AddRangeAsync(It.IsAny<IEnumerable<IOutboxMessage>>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}

// ============================================================
// OUTBOX MESSAGE TESTS
// ============================================================

public class OutboxMessageTests
{
    [Fact]
    public void OutboxMessage_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var message = new OutboxMessage();

        // Assert
        message.Id.Should().NotBeEmpty();
        message.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        message.ProcessedAt.Should().BeNull();
        message.Attempts.Should().Be(0);
        message.LastError.Should().BeNull();
    }

    [Fact]
    public void OutboxMessage_ShouldStoreProperties()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var occurredAt = DateTimeOffset.UtcNow;

        // Act
        var message = new OutboxMessage
        {
            EventId = eventId,
            EventType = "TestEvent",
            Payload = "{\"test\":true}",
            OccurredAt = occurredAt
        };

        // Assert
        message.EventId.Should().Be(eventId);
        message.EventType.Should().Be("TestEvent");
        message.Payload.Should().Be("{\"test\":true}");
        message.OccurredAt.Should().Be(occurredAt);
    }
}

// ============================================================
// EVENT BASE TESTS
// ============================================================

public class EventBaseTests
{
    [Fact]
    public void EventBase_ShouldHaveUniqueEventId()
    {
        // Arrange & Act
        var event1 = new TestEvent("Test 1");
        var event2 = new TestEvent("Test 2");

        // Assert
        event1.EventId.Should().NotBeEmpty();
        event2.EventId.Should().NotBeEmpty();
        event1.EventId.Should().NotBe(event2.EventId);
    }

    [Fact]
    public void EventBase_ShouldHaveOccurredAtCloseToNow()
    {
        // Arrange & Act
        var @event = new TestEvent("Test");

        // Assert
        @event.OccurredAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void EventBase_ShouldImplementIEvent()
    {
        // Arrange & Act
        var @event = new TestEvent("Test");

        // Assert
        @event.Should().BeAssignableTo<IEvent>();
    }
}

// ============================================================
// EXTERNAL EVENT PUBLISHER OPTIONS TESTS
// ============================================================

public class ExternalEventPublisherOptionsTests
{
    [Fact]
    public void Options_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var options = new ExternalEventPublisherOptions();

        // Assert
        options.TopicName.Should().BeNull();
        options.IncludeEventType.Should().BeTrue();
        options.SerializerType.Should().Be(EventSerializerType.SystemTextJson);
    }
}
