namespace Marketplace.Web.Models.Shared;

public sealed class TaxonomySectionVm<TItem>
{
    public SectionHeaderVm Header { get; set; } = new();
    public IReadOnlyCollection<TItem> Items { get; set; } = Array.Empty<TItem>();

    public bool HasItems => Items.Count > 0;
}