using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Models.Shared;

public sealed class ListingsSectionVm
{
    public SectionHeaderVm Header { get; set; } = new();
    public CatalogFilterVm Filter { get; set; } = new();
    public IReadOnlyCollection<ListingCardVm> Listings { get; set; } = Array.Empty<ListingCardVm>();
    public PaginationVm Pagination { get; set; } = new();

    public bool ShowMobileFilterButton { get; set; } = true;
}