namespace Marketplace.Web.Areas.Admin.Models.Regions;

public class RegionListItemVm
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public int CitiesCount { get; init; }
}