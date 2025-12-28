using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eQuantic.Core.Eventing.AWS;

/// <summary>
/// Extension methods for registering AWS event services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds AWS SNS event publisher to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Action to configure options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddAwsSnsPublisher(
        this IServiceCollection services,
        Action<SnsEventPublisherOptions> configure)
    {
        services.Configure(configure);

        services.AddSingleton<IAmazonSimpleNotificationService>(sp =>
        {
            var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<SnsEventPublisherOptions>>().Value;
            
            if (string.IsNullOrEmpty(options.Region))
                return new AmazonSimpleNotificationServiceClient();

            return new AmazonSimpleNotificationServiceClient(RegionEndpoint.GetBySystemName(options.Region));
        });

        services.AddSingleton<IExternalEventPublisher, SnsEventPublisher>();

        return services;
    }

    /// <summary>
    /// Adds AWS SQS event subscriber to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Action to configure options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddAwsSqsSubscriber(
        this IServiceCollection services,
        Action<SqsEventSubscriberOptions> configure)
    {
        services.Configure(configure);

        services.AddSingleton<IAmazonSQS>(sp =>
        {
            var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<SqsEventSubscriberOptions>>().Value;
            
            if (string.IsNullOrEmpty(options.Region))
                return new AmazonSQSClient();

            return new AmazonSQSClient(RegionEndpoint.GetBySystemName(options.Region));
        });

        services.AddSingleton<IExternalEventSubscriber, SqsEventSubscriber>();
        services.AddHostedService<SqsSubscriberHostedService>();

        return services;
    }
}

/// <summary>
/// Hosted service that manages the SQS subscriber lifecycle.
/// </summary>
public class SqsSubscriberHostedService : IHostedService
{
    private readonly IExternalEventSubscriber _subscriber;

    public SqsSubscriberHostedService(IExternalEventSubscriber subscriber)
    {
        _subscriber = subscriber;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _subscriber.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _subscriber.StopAsync(cancellationToken);
    }
}
