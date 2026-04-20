namespace Marketplace.Web.Models.Common;

public abstract class BaseFilter
{
    public string? Search { get; set; }
    public string? Sort { get; set; }
}