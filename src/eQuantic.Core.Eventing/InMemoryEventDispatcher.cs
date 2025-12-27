using Microsoft.Extensions.DependencyInjection;

namespace eQuantic.Core.Eventing;

/// <summary>
/// In-memory event dispatcher that resolves handlers from DI container.
/// </summary>
public class InMemoryEventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly EventDispatchStrategy _strategy;

    /// <summary>
    /// Creates a new instance of InMemoryEventDispatcher.
    /// </summary>
    /// <param name="serviceProvider">The service provider for resolving handlers.</param>
    /// <param name="strategy">The dispatch strategy to use.</param>
    public InMemoryEventDispatcher(
        IServiceProvider serviceProvider,
        EventDispatchStrategy strategy = EventDispatchStrategy.WhenAll)
    {
        _serviceProvider = serviceProvider;
        _strategy = strategy;
    }

    /// <inheritdoc />
    public async Task DispatchAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
        
        switch (_strategy)
        {
            case EventDispatchStrategy.WhenAll:
                await Task.WhenAll(handlers.Select(h => h.HandleAsync(@event, cancellationToken)));
                break;

            case EventDispatchStrategy.StopOnFirstException:
                foreach (var handler in handlers)
                {
                    await handler.HandleAsync(@event, cancellationToken);
                }
                break;

            case EventDispatchStrategy.ContinueOnException:
                var exceptions = new List<Exception>();
                foreach (var handler in handlers)
                {
                    try
                    {
                        await handler.HandleAsync(@event, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }
                if (exceptions.Count > 0)
                {
                    throw new AggregateException("One or more event handlers failed", exceptions);
                }
                break;
        }
    }

    /// <inheritdoc />
    public async Task DispatchAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var @event in events)
        {
            var eventType = @event.GetType();
            var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                var method = handlerType.GetMethod(nameof(IEventHandler<IEvent>.HandleAsync));
                if (method != null)
                {
                    var task = (Task)method.Invoke(handler, new object[] { @event, cancellationToken })!;
                    await task;
                }
            }
        }
    }
}
