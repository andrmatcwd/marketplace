namespace Marketplace.Web.Models.Home;

public sealed class HomeHeroVm
{
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }

    public string SearchActionUrl { get; set; } = string.Empty;

    public string SearchPlaceholder { get; set; } = "Яку послугу ви шукаєте?";
    public string CityPlaceholder { get; set; } = "Оберіть місто";
}