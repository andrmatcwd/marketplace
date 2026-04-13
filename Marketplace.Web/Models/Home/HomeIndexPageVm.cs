using System;
using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.City;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Models.Home;

public class HomeIndexPageVm
{
    public string H1 { get; init; } = default!;
    public string? IntroText { get; init; }

    public IReadOnlyCollection<CityCardVm> Cities { get; init; }
        = Array.Empty<CityCardVm>();

    public IReadOnlyCollection<CategoryCardVm> Categories { get; init; }
        = Array.Empty<CategoryCardVm>();

    public IReadOnlyCollection<ListingCardVm> Listings { get; init; }
        = Array.Empty<ListingCardVm>();

    public IReadOnlyCollection<BreadcrumbItemVm> Breadcrumbs { get; init; }
        = Array.Empty<BreadcrumbItemVm>();
}
