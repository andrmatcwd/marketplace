using Marketplace.Web.Data;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Shared;
using Marketplace.Web.Navigation;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Services.Catalog;

public sealed class CatalogFilterEnricher : ICatalogFilterEnricher
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICatalogVmMapper _mapper;
    private readonly ICatalogUrlBuilder _urlBuilder;

    public CatalogFilterEnricher(
        ApplicationDbContext dbContext,
        ICatalogVmMapper mapper,
        ICatalogUrlBuilder urlBuilder)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _urlBuilder = urlBuilder;
    }

    public async Task<CatalogFilterVm> EnrichAsync(
        string culture,
        CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        filter.Page = filter.Page < 1 ? 1 : filter.Page;
        filter.PageSize = filter.PageSize <= 0 ? 12 : Math.Min(filter.PageSize, 60);

        var cities = await _dbContext.Cities
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new { x.Slug, x.Name })
            .ToListAsync(cancellationToken);

        var categories = await _dbContext.Categories
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new { x.Slug, x.Name })
            .ToListAsync(cancellationToken);

        filter.Cities = cities
            .Select(x => _mapper.MapFilterOption(x.Slug, x.Name))
            .ToList();

        filter.Categories = categories
            .Select(x => _mapper.MapFilterOption(x.Slug, x.Name))
            .ToList();

        filter.SortOptions = new List<FilterOptionVm>
        {
            _mapper.MapFilterOption("newest", "Newest first"),
            _mapper.MapFilterOption("rating", "By rating"),
            _mapper.MapFilterOption("title", "By title")
        };

        filter.ResetUrl = _urlBuilder.BuildCatalogUrl(culture);

        return filter;
    }
}