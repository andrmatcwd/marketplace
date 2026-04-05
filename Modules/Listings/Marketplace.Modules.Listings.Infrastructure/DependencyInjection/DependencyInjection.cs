using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Listings.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddListingsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddListingsDbContext(configuration);
        services.AddListingsRepositories();
        services.AddListingsServices();
        services.AddListingsMediatR();

        return services;
    }
}
