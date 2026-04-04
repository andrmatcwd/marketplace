using System;
using Marketplace.Modules.Listings.Application.Services;
using Marketplace.Modules.Listings.Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Listings.Infrastructure.DependencyInjection;

public static class ServiceInjection
{
    public static IServiceCollection AddListingsServices(this IServiceCollection services)
    {
        services.AddScoped<IListingService, ListingService>();
        return services;
    }
}
