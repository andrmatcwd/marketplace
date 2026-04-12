namespace Marketplace.Web.Areas.Admin.Models.Cities;

public class CityListItemVm
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public int RegionId { get; init; }
    public string RegionName { get; init; } = string.Empty;
    public int ListingsCount { get; init; }
}