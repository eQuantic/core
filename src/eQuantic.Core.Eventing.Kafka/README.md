# eQuantic.Core.Eventing.Kafka

Apache Kafka integration for eQuantic.Core.Eventing using Confluent.Kafka.

## Installation

```bash
dotnet add package eQuantic.Core.Eventing.Kafka
```

## Usage

### Publisher

```csharp
services.AddEventDispatcher();
services.AddKafkaPublisher(options =>
{
    options.BootstrapServers = "localhost:9092";
    options.TopicName = "domain-events";
    options.Acks = Acks.All;
});
```

### Subscriber

```csharp
services.AddEventDispatcher();
services.AddKafkaSubscriber(options =>
{
    options.BootstrapServers = "localhost:9092";
    options.Topics = new List<string> { "domain-events" };
    options.GroupId = "my-service-consumer-group";
    options.AutoOffsetReset = AutoOffsetReset.Earliest;
    options.EventTypeResolver = typeName => Type.GetType(typeName);
});

// Register event handlers
services.AddTransient<IEventHandler<OrderPlacedEvent>, OrderPlacedHandler>();
```

## Features

- **Producer**: Publishes events with headers (EventType, EventId, OccurredAt)
- **Consumer**: Consumes messages with manual or auto commit
- **Serialization**: JSON with configurable options
- **Partitioning**: Custom key selector for message partitioning
