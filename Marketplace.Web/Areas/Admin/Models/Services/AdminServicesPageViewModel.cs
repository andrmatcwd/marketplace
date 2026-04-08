using Marketplace.Web.Models.Services;

namespace Marketplace.Web.Areas.Admin.Models.Services;

public sealed class AdminServicesPageViewModel
{
    public AdminServicesFilterViewModel Filter { get; set; } = new();
    public IReadOnlyList<ServiceItemViewModel> Items { get; set; } = [];
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}
