namespace Marketplace.Web.Models.Shared;

public sealed class SeoIntroVm
{
    public string? Title { get; set; }
    public string? Text { get; set; }

    public bool HasContent =>
        !string.IsNullOrWhiteSpace(Title) ||
        !string.IsNullOrWhiteSpace(Text);
}