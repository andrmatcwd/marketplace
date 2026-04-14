namespace Marketplace.Web.Models.Shared;

public sealed class SectionHeaderVm
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Meta { get; set; }
}