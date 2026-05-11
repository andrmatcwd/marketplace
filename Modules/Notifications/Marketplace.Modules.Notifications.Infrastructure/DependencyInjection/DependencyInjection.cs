using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Notifications.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddNotificationsDbContext(configuration);
        services.AddNotificationsRepositories();
        services.AddNotificationsServices(configuration);
        services.AddNotificationsMediatR();
        services.AddNotificationsAutoMapper();

        return services;
    }
}
