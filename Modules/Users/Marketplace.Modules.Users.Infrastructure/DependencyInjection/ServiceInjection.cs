using Marketplace.Modules.Users.Application.Services;
using Marketplace.Modules.Users.Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Users.Infrastructure.DependencyInjection;

public static class ServiceInjection
{
    public static IServiceCollection AddUsersServices(this IServiceCollection services)
    {
        services.AddScoped<IAppUserService, AppUserService>();

        return services;
    }
}
