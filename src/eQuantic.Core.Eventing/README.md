# eQuantic.Core.Eventing

[![NuGet](https://img.shields.io/nuget/v/eQuantic.Core.Eventing.svg)](https://www.nuget.org/packages/eQuantic.Core.Eventing/)

Base eventing abstractions for the eQuantic ecosystem. Provides shared interfaces for event-driven architectures.

## Installation

```bash
dotnet add package eQuantic.Core.Eventing
```

## Core Interfaces

| Interface               | Description                       |
| ----------------------- | --------------------------------- |
| `IEvent`                | Base marker for all events        |
| `IEventHandler<TEvent>` | Handler for a specific event type |
| `IEventDispatcher`      | Dispatches events to handlers     |
| `IEventSource`          | Entity that can raise events      |

## Usage

### Define an Event

```csharp
public record OrderPlacedEvent : EventBase
{
    public Guid OrderId { get; init; }
    public decimal TotalAmount { get; init; }
}
```

### Create a Handler

```csharp
public class OrderPlacedHandler : IEventHandler<OrderPlacedEvent>
{
    public async Task HandleAsync(OrderPlacedEvent @event, CancellationToken ct)
    {
        // Handle the event
        Console.WriteLine($"Order {event.OrderId} placed for ${event.TotalAmount}");
    }
}
```

### Register Services

```csharp
services.AddEventDispatcher(EventDispatchStrategy.WhenAll);
services.AddEventHandler<OrderPlacedEvent, OrderPlacedHandler>();
```

### Dispatch Events

```csharp
await dispatcher.DispatchAsync(new OrderPlacedEvent
{
    OrderId = Guid.NewGuid(),
    TotalAmount = 99.99m
});
```

### Event-Sourced Entities

```csharp
public class Order : EventSourceBase
{
    public void Place(decimal amount)
    {
        // Business logic...
        AddEvent(new OrderPlacedEvent { OrderId = Id, TotalAmount = amount });
    }
}

// After saving:
var events = order.GetUncommittedEvents();
await dispatcher.DispatchAsync(events);
order.ClearUncommittedEvents();
```

## Ecosystem Integration

This package is the foundation for:

- **eQuantic.Core.CQS** - `INotification : IEvent`
- **eQuantic.Core.DomainEvents** - `IDomainEvent : IEvent`
