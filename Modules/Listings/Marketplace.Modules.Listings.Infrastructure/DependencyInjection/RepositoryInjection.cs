using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Listings.Infrastructure.DependencyInjection;

public static class RepositoryInjection
{
    public static IServiceCollection AddListingsRepositories(this IServiceCollection services)
    {
        services.AddScoped<IListingRepository, ListingRepository>();
        return services;
    }
}
