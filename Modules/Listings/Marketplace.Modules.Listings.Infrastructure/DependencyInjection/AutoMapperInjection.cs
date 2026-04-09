using Marketplace.Modules.Listings.Application.Listings.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Listings.Infrastructure.DependencyInjection;

public static class AutoMapperInjection
{
    public static IServiceCollection AddListingsAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ListingMappingProfile).Assembly);

        return services;
    }
}
