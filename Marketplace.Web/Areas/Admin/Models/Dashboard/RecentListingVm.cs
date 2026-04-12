namespace Marketplace.Web.Areas.Admin.Models.Dashboard;

public class RecentListingVm
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string SubCategory { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public DateTime CreatedUtc { get; init; }
}