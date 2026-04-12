using System;

namespace Marketplace.Web.Models;

public class ListingUrlVm
{
    public int Id { get; set; }
    public string CitySlug { get; set; } = default!;
    public string CategorySlug { get; set; } = default!;
    public string SubcategorySlug { get; set; } = default!;
    public string ListingSlug { get; set; } = default!;
}
