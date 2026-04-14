namespace Marketplace.Web.Models.Shared;

public sealed class PageHeroVm
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public IReadOnlyCollection<BreadcrumbItemVm> Breadcrumbs { get; set; } = Array.Empty<BreadcrumbItemVm>();
}