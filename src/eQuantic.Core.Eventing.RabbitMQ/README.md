# eQuantic.Core.Eventing.RabbitMQ

RabbitMQ integration for eQuantic.Core.Eventing.

## Installation

```bash
dotnet add package eQuantic.Core.Eventing.RabbitMQ
```

## Usage

```csharp
services.AddRabbitMqPublisher(options =>
{
    options.HostName = "localhost";
    options.Port = 5672;
    options.UserName = "guest";
    options.Password = "guest";
    options.ExchangeName = "domain-events";
    options.ExchangeType = "topic";
    options.UseEventTypeAsRoutingKey = true;
});
```
