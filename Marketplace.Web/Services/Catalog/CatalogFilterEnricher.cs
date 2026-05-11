using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Shared;
using Marketplace.Web.Navigation;
using MediatR;

namespace Marketplace.Web.Services.Catalog;

public sealed class CatalogFilterEnricher : ICatalogFilterEnricher
{
    private readonly IMediator _mediator;
    private readonly ICatalogVmMapper _mapper;
    private readonly ICatalogUrlBuilder _urlBuilder;

    public CatalogFilterEnricher(
        IMediator mediator,
        ICatalogVmMapper mapper,
        ICatalogUrlBuilder urlBuilder)
    {
        _mediator = mediator;
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

        var cities = await _mediator.Send(new GetCatalogCitiesQuery(), cancellationToken);
        var categories = await _mediator.Send(new GetCatalogCategoriesQuery(), cancellationToken);

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
