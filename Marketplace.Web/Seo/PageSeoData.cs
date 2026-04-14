namespace Marketplace.Web.Seo;

public sealed class PageSeoData
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? CanonicalUrl { get; set; }
    public string? Robots { get; set; } = "index, follow";

    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
    public string? OgType { get; set; } = "website";
    public string? OgUrl { get; set; }

    public string? TwitterCard { get; set; } = "summary_large_image";
    public string? TwitterTitle { get; set; }
    public string? TwitterDescription { get; set; }
    public string? TwitterImage { get; set; }

    public string? H1 { get; set; }
    public string? SeoText { get; set; }

    public string? PrevUrl { get; set; }
    public string? NextUrl { get; set; }

    public IReadOnlyCollection<HreflangLink> Hreflangs { get; set; } = Array.Empty<HreflangLink>();
}

public sealed class HreflangLink
{
    public string Lang { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}