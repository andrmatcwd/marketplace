using System;

namespace Marketplace.Web.Seo;

public class PageSeoData
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? CanonicalUrl { get; set; }
    public string? Robots { get; set; }
    public List<string> JsonLdBlocks { get; set; } = new();
}
