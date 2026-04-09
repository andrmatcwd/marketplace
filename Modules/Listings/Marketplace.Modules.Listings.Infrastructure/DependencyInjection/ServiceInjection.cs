using Marketplace.Modules.Listings.Application.Services;
using Marketplace.Modules.Listings.Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Listings.Infrastructure.DependencyInjection;

public static class ServiceInjection
{
    public static IServiceCollection AddListingsServices(this IServiceCollection services)
    {
        services.AddScoped<IListingService, ListingService>();
        services.AddScoped<IRegionService, RegionService>();
        services.AddScoped<IReviewerService, ReviewerService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<ILocationService, LocationService>();

        return services;
    }
}
