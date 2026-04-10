using System;

namespace Marketplace.Web.Models.Listings;

public class ListingImageVm
{
    public string Url { get; init; } = default!;
    public string? Alt { get; init; }
    public int SortOrder { get; init; }
}
