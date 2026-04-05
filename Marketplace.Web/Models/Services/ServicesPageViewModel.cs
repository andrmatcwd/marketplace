namespace Marketplace.Web.Models.Services;

public sealed class ServicesPageViewModel
{
    public ServicesFilterRequest Filters { get; set; } = new();
    public IReadOnlyList<ServiceCategoryViewModel> Categories { get; set; } = [];
    public PagedResult<ServiceItemViewModel> Results { get; set; } = new();
}