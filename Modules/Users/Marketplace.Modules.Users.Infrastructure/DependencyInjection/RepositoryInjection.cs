using Marketplace.Modules.Users.Application.Repositories;
using Marketplace.Modules.Users.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Users.Infrastructure.DependencyInjection;

public static class RepositoryInjection
{
    public static IServiceCollection AddUsersRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAppUserRepository, AppUserRepository>();

        return services;
    }
}
