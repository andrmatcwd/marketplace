using Marketplace.Modules.Users.Application.Users.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Users.Infrastructure.DependencyInjection;

public static class AutoMapperInjection
{
    public static IServiceCollection AddUsersAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMappingProfile).Assembly);

        return services;
    }
}
