namespace Marketplace.Web.Options;

public sealed class UiOptions
{
    public string SiteName { get; set; } = "Marketplace";
    public int DefaultPageSize { get; set; } = 3;
    public int MaxPageSize { get; set; } = 60;
}