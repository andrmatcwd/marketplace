using Marketplace.Modules.Notifications.Application.Repositories;
using Marketplace.Modules.Notifications.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Notifications.Infrastructure.DependencyInjection;

public static class RepositoryInjection
{
    public static IServiceCollection AddNotificationsRepositories(this IServiceCollection services)
    {
        services.AddScoped<INotificationRepository, NotificationRepository>();

        return services;
    }
}
