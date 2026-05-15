namespace Marketplace.Web.Models.MyListings;

public class MyListingIndexVm
{
    public IReadOnlyCollection<MyListingListItemVm> Items { get; set; } = [];
    public int TotalCount { get; set; }
}
