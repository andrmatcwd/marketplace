using Marketplace.Modules.Notifications.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Notifications.Infrastructure.DependencyInjection;

public static class DbContextInjection
{
    public static IServiceCollection AddNotificationsDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<NotificationsDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }
}
