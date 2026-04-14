using Marketplace.Web.Data;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Navigation;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Services.Listings;

public sealed class ListingService : IListingService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IListingVmMapper _mapper;
    private readonly ICatalogBreadcrumbBuilder _breadcrumbBuilder;

    public ListingService(
        ApplicationDbContext dbContext,
        IListingVmMapper mapper,
        ICatalogBreadcrumbBuilder breadcrumbBuilder)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _breadcrumbBuilder = breadcrumbBuilder;
    }

    public async Task<ListingDetailsPageVm?> GetDetailsPageAsync(
        string culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        string serviceSlug,
        Guid id,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Listings
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.SubCategory)
            .Include(x => x.City)
            .Include(x => x.Images)
            .Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.IsPublished, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        if (entity.City is null || entity.Category is null || entity.SubCategory is null)
        {
            return null;
        }

        var relatedEntities = await _dbContext.Listings
            .AsNoTracking()
            .Include(x => x.City)
            .Include(x => x.Category)
            .Include(x => x.SubCategory)
            .Include(x => x.Images)
            .Where(x =>
                x.IsPublished &&
                x.Id != entity.Id &&
                x.CityId == entity.CityId &&
                x.SubCategoryId == entity.SubCategoryId)
            .OrderByDescending(x => x.Rating)
            //.ThenByDescending(x => x.CreatedAtUtc)
            .Take(6)
            .ToListAsync(cancellationToken);

        var relatedListings = relatedEntities
            .Select(x => _mapper.MapRelatedListing(x, culture))
            .ToList();

        var vm = _mapper.MapDetails(entity, culture, relatedListings);

        vm.Breadcrumbs = _breadcrumbBuilder.BuildListing(
            culture,
            entity.Title,
            entity.City.Name,
            entity.City.Slug,
            entity.Category.Name,
            entity.Category.Slug,
            entity.SubCategory.Name,
            entity.SubCategory.Slug);

        return vm;
    }
}