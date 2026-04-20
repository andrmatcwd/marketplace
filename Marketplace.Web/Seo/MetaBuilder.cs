namespace Marketplace.Web.Seo;

public sealed class MetaBuilder
{
    public PageSeoData BuildBasic(string title, string? description = null)
    {
        return new PageSeoData
        {
            Title = title,
            Description = description,
            OgTitle = title,
            OgDescription = description
        };
    }
}