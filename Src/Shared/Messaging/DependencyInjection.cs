using Microsoft.Extensions.DependencyInjection;
using Messaging.Publish;
using Messaging.Consume;

namespace Messaging;

public static class DependencyInjection
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<IPublishEventBus, PublishEventBus>();
        services.AddSingleton<IConsumeEventBus, ConsumeEventBus>();

        return services;
    }
}
