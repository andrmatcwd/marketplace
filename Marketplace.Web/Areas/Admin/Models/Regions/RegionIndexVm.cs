namespace Marketplace.Web.Areas.Admin.Models.Regions;

public class RegionIndexVm
{
    public string? Search { get; set; }
    public IReadOnlyCollection<RegionListItemVm> Items { get; init; } = [];
}