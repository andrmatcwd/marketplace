using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Listings.Infrastructure.DependencyInjection;

public static class RepositoryInjection
{
    public static IServiceCollection AddListingsRepositories(this IServiceCollection services)
    {
        services.AddScoped<IListingRepository, ListingRepository>();
        services.AddScoped<IReviewerRepository, ReviewerRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IListingVacancyRepository, ListingVacancyRepository>();
        services.AddScoped<IListingRentalRepository, ListingRentalRepository>();

        return services;
    }
}
