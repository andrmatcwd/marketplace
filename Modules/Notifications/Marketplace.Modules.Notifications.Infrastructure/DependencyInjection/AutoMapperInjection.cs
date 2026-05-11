using Marketplace.Modules.Notifications.Application.Notifications.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Notifications.Infrastructure.DependencyInjection;

public static class AutoMapperInjection
{
    public static IServiceCollection AddNotificationsAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(NotificationMappingProfile).Assembly);

        return services;
    }
}
