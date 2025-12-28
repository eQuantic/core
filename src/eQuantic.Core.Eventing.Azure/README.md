# eQuantic.Core.Eventing.Azure

Azure Service Bus integration for eQuantic.Core.Eventing.

## Installation

```bash
dotnet add package eQuantic.Core.Eventing.Azure
```

## Usage

```csharp
services.AddAzureServiceBusPublisher(options =>
{
    options.ConnectionString = "your-connection-string";
    options.TopicName = "domain-events";
    options.IncludeEventType = true;
});
```

Then inject `IExternalEventPublisher` in your services:

```csharp
public class MyService
{
    private readonly IExternalEventPublisher _publisher;

    public MyService(IExternalEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task DoSomething()
    {
        await _publisher.PublishAsync(new MyEvent());
    }
}
```
