using Marketplace.Modules.Notifications.Application.Notifications.Providers;
using Marketplace.Modules.Notifications.Application.Services;
using Marketplace.Modules.Notifications.Application.Services.Implementations;
using Marketplace.Modules.Notifications.Infrastructure.Options;
using Marketplace.Modules.Notifications.Infrastructure.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Notifications.Infrastructure.DependencyInjection;

public static class ServiceInjection
{
    public static IServiceCollection AddNotificationsServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<INotificationService, NotificationService>();

        services.Configure<ContactNotificationOptions>(
            configuration.GetSection("ContactNotifications"));

        services.AddTransient<INotificationProvider, TelegramNotificationProvider>();
        services.AddTransient<INotificationProvider, EmailNotificationProvider>();

        return services;
    }
}
