# eQuantic.Core.Eventing.Outbox

Outbox pattern abstractions for eQuantic.Core.Eventing. Provides transactional event publishing with guaranteed delivery.

## Installation

```bash
dotnet add package eQuantic.Core.Eventing.Outbox
```

## Usage

```csharp
// Register with your IOutboxStore implementation
services.AddOutbox<MyOutboxStore>(options =>
{
    options.BatchSize = 100;
    options.ProcessingInterval = TimeSpan.FromSeconds(5);
    options.MaxRetryAttempts = 3;
    options.RetentionPeriod = TimeSpan.FromDays(7);
});

// Register an external publisher (Azure, AWS, RabbitMQ)
services.AddAzureServiceBusPublisher(options => ...);
```

The `OutboxProcessorService` will automatically:

1. Pick up pending messages from the outbox
2. Publish them using the registered `IExternalEventPublisher`
3. Mark them as processed or failed
4. Clean up old processed messages

## Implementing IOutboxStore

For Entity Framework Core, use `eQuantic.Core.Eventing.Outbox.EF` package.

For custom implementations:

```csharp
public class MyOutboxStore : IOutboxStore
{
    public Task AddAsync(IOutboxMessage message, CancellationToken ct) { ... }
    public Task<IReadOnlyList<IOutboxMessage>> GetPendingAsync(int batchSize, CancellationToken ct) { ... }
    public Task MarkAsProcessedAsync(Guid id, CancellationToken ct) { ... }
    // ...
}
```
