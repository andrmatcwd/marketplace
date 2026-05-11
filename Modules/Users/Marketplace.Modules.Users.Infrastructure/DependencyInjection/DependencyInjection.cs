using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Users.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddUsersDbContext(configuration);
        services.AddUsersRepositories();
        services.AddUsersServices();
        services.AddUsersMediatR();
        services.AddUsersAutoMapper();

        return services;
    }
}
