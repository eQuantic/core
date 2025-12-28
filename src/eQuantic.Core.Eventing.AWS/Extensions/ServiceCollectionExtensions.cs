using Amazon;
using Amazon.SimpleNotificationService;
using Microsoft.Extensions.DependencyInjection;

namespace eQuantic.Core.Eventing.AWS;

/// <summary>
/// Extension methods for registering AWS SNS event publisher.
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
}
