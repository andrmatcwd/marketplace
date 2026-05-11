using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers.Api;

[ApiController]
[Route("{culture:regex(^uk|en$)}/api/listings")]
public sealed class ListingsApiController : ControllerBase
{
    private readonly IMediator _mediator;

    public ListingsApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        string culture,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);

        var listingFilter = new CatalogListingFilter
        {
            Search = filter.Search,
            CitySlug = filter.City,
            CategorySlug = filter.Category,
            Sort = filter.Sort,
            Page = filter.Page,
            PageSize = filter.PageSize
        };

        var listings = await _mediator.Send(new GetCatalogListingsQuery(listingFilter), cancellationToken);

        var payload = listings.Select(x => new
        {
            x.Id,
            x.Title,
            Url = $"/{culture}/{x.CitySlug}/{x.CategorySlug}/{x.SubCategorySlug}/{x.Slug}/{x.Id}",
            x.CityName,
            x.CategoryName,
            x.SubCategoryName,
            x.ShortDescription,
            x.Rating,
            x.ReviewsCount
        });

        return Ok(payload);
    }

    [HttpGet("autocomplete")]
    public async Task<IActionResult> AutoComplete(
        string culture,
        [FromQuery] string? search,
        [FromQuery] string? city,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);

        if (string.IsNullOrWhiteSpace(search))
            return Ok(Array.Empty<object>());

        var trimmedSearch = search.Trim();
        var trimmedCity = city?.Trim();

        if (!string.IsNullOrWhiteSpace(trimmedCity))
        {
            var cityId = await _mediator.Send(new GetCityIdBySlugQuery(trimmedCity), cancellationToken);

            if (cityId.HasValue)
            {
                var subCategorySuggestions = await _mediator.Send(
                    new SearchSubCategoriesInCityQuery(cityId.Value, trimmedSearch, 5), cancellationToken);

                var subCategoryPayload = subCategorySuggestions
                    .Select(x => new
                    {
                        type = "subcategory",
                        label = x.Name,
                        url = $"/{culture}/{trimmedCity}/{x.CategorySlug}/{x.Slug}",
                        city = trimmedCity,
                        category = x.CategoryName,
                        score = x.Score,
                        listingsCount = x.ListingsCount
                    });

                var listingFilter = new CatalogListingFilter
                {
                    Search = trimmedSearch,
                    CitySlug = trimmedCity,
                    Page = 1,
                    PageSize = 5
                };

                var listingResults = await _mediator.Send(new GetCatalogListingsQuery(listingFilter), cancellationToken);

                var listingSuggestions = listingResults
                    .Select(x => new
                    {
                        type = "listing",
                        label = x.Title,
                        url = $"/{culture}/{x.CitySlug}/{x.CategorySlug}/{x.SubCategorySlug}/{x.Slug}/{x.Id}",
                        city = x.CityName,
                        category = x.SubCategoryName ?? x.CategoryName
                    })
                    .Take(5);

                return Ok(subCategoryPayload.Cast<object>().Concat(listingSuggestions).Take(8));
            }
        }

        var fallbackFilter = new CatalogListingFilter
        {
            Search = trimmedSearch,
            Page = 1,
            PageSize = 8
        };

        var fallbackResults = await _mediator.Send(new GetCatalogListingsQuery(fallbackFilter), cancellationToken);

        return Ok(fallbackResults.Select(x => new
        {
            type = "listing",
            label = x.Title,
            url = $"/{culture}/{x.CitySlug}/{x.CategorySlug}/{x.SubCategorySlug}/{x.Slug}/{x.Id}",
            city = x.CityName,
            category = x.SubCategoryName ?? x.CategoryName
        }).Take(8));
    }

    [HttpGet("resolve-route")]
    public async Task<IActionResult> ResolveRoute(
        string culture,
        [FromQuery] string? city,
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);

        if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(search))
            return Ok(new { canRouteDirect = false });

        var trimmedCity = city.Trim();
        var trimmedSearch = search.Trim();

        var cityId = await _mediator.Send(new GetCityIdBySlugQuery(trimmedCity), cancellationToken);
        if (!cityId.HasValue)
            return Ok(new { canRouteDirect = false });

        var matches = await _mediator.Send(
            new SearchSubCategoriesInCityQuery(cityId.Value, trimmedSearch, 1), cancellationToken);

        var match = matches.FirstOrDefault();

        if (match is null || match.Score <= 0)
            return Ok(new { canRouteDirect = false });

        return Ok(new
        {
            canRouteDirect = true,
            type = "subcategory",
            url = $"/{culture}/{trimmedCity}/{match.CategorySlug}/{match.Slug}",
            categoryName = match.CategoryName,
            subCategoryName = match.Name
        });
    }
}
