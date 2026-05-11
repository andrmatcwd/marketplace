using Marketplace.Modules.Listings.Application.Catalog.Services;
using Marketplace.Modules.Listings.Application.Services;
using Marketplace.Modules.Listings.Application.Services.Implementations;
using Marketplace.Modules.Listings.Infrastructure.Catalog;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Listings.Infrastructure.DependencyInjection;

public static class ServiceInjection
{
    public static IServiceCollection AddListingsServices(this IServiceCollection services)
    {
        services.AddScoped<IListingService, ListingService>();
        services.AddScoped<IReviewerService, ReviewerService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<ICityService, CityService>();

        services.AddScoped<IListingVacancyService, ListingVacancyService>();
        services.AddScoped<IListingRentalService, ListingRentalService>();

        services.AddScoped<ISlugService, SlugService>();
        services.AddScoped<ICatalogDataService, CatalogDataService>();

        return services;
    }
}
