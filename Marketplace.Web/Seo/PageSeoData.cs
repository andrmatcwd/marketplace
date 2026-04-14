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

    public static PageSeoData CreateDefault(string title, string? description = null)
    {
        return new PageSeoData
        {
            Title = title,
            Description = description,
            OgTitle = title,
            OgDescription = description
        };
    }

    public static PageSeoData CreateNoIndex(string title, string? description = null)
    {
        return new PageSeoData
        {
            Title = title,
            Description = description,
            Robots = "noindex, nofollow",
            OgTitle = title,
            OgDescription = description
        };
    }
}