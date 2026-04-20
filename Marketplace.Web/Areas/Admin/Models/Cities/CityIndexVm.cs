namespace Marketplace.Web.Areas.Admin.Models.Cities;

public class CityIndexVm
{
    public string? Search { get; set; }
    public int? RegionId { get; set; }
    public IReadOnlyCollection<CityListItemVm> Items { get; init; } = [];
}