namespace Marketplace.Web.Areas.Admin.Models.Dashboard;

public class DashboardVm
{
    public int RegionsCount { get; init; }
    public int CitiesCount { get; init; }
    public int CategoriesCount { get; init; }
    public int SubCategoriesCount { get; init; }
    public int ListingsCount { get; init; }

    public IReadOnlyCollection<RecentListingVm> RecentListings { get; init; } = [];
}