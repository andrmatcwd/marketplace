namespace Marketplace.Web.Models.Shared;

public sealed class BreadcrumbItemVm
{
    public string Title { get; set; } = string.Empty;
    public string? Url { get; set; }
}