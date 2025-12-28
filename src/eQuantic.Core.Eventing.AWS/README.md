# eQuantic.Core.Eventing.AWS

AWS SNS integration for eQuantic.Core.Eventing.

## Installation

```bash
dotnet add package eQuantic.Core.Eventing.AWS
```

## Usage

```csharp
services.AddAwsSnsPublisher(options =>
{
    options.TopicArn = "arn:aws:sns:us-east-1:123456789:domain-events";
    options.Region = "us-east-1";
    options.IncludeEventType = true;
});
```

For FIFO topics:

```csharp
services.AddAwsSnsPublisher(options =>
{
    options.TopicArn = "arn:aws:sns:us-east-1:123456789:domain-events.fifo";
    options.MessageGroupId = "my-group";
    options.UseFifoDeduplication = true;
});
```
