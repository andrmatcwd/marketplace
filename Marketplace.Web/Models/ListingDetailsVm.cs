using System;

namespace Marketplace.Web.Models;

public class ListingDetailsVm
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string CityName { get; set; } = default!;
    public string CitySlug { get; set; } = default!;

    public string CategoryName { get; set; } = default!;
    public string CategorySlug { get; set; } = default!;

    public string SubcategoryName { get; set; } = default!;
    public string SubcategorySlug { get; set; } = default!;

    public string ListingSlug { get; set; } = default!;
}
