using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Notifications.Infrastructure.DependencyInjection;

public static class MediatRInjection
{
    public static IServiceCollection AddNotificationsMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(Application.Notifications.Commands.CreateNotification.CreateNotificationCommand).Assembly));

        return services;
    }
}
