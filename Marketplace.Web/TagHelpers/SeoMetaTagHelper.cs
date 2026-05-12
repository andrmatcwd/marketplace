using Marketplace.Web.Seo;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Net;

namespace Marketplace.Web.TagHelpers;

[HtmlTargetElement("seo-meta", TagStructure = TagStructure.WithoutEndTag)]
public sealed class SeoMetaTagHelper : TagHelper
{
    public PageSeoData? Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;

        if (Model is null)
        {
            output.Content.SetHtmlContent(string.Empty);
            return;
        }

        var html = new List<string>();

        if (!string.IsNullOrWhiteSpace(Model.Description))
        {
            html.Add($"""<meta name="description" content="{Encode(Model.Description)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.Robots))
        {
            html.Add($"""<meta name="robots" content="{Encode(Model.Robots)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.CanonicalUrl))
        {
            html.Add($"""<link rel="canonical" href="{Encode(Model.CanonicalUrl)}" />""");
        }

        foreach (var link in Model.Hreflangs)
        {
            html.Add($"""<link rel="alternate" hreflang="{Encode(link.Lang)}" href="{Encode(link.Url)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.OgLocale))
        {
            html.Add($"""<meta property="og:locale" content="{Encode(Model.OgLocale)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.OgSiteName))
        {
            html.Add($"""<meta property="og:site_name" content="{Encode(Model.OgSiteName)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.OgTitle))
        {
            html.Add($"""<meta property="og:title" content="{Encode(Model.OgTitle)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.OgDescription))
        {
            html.Add($"""<meta property="og:description" content="{Encode(Model.OgDescription)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.OgImage))
        {
            html.Add($"""<meta property="og:image" content="{Encode(Model.OgImage)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.OgType))
        {
            html.Add($"""<meta property="og:type" content="{Encode(Model.OgType)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.OgUrl))
        {
            html.Add($"""<meta property="og:url" content="{Encode(Model.OgUrl)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.TwitterCard))
        {
            html.Add($"""<meta name="twitter:card" content="{Encode(Model.TwitterCard)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.TwitterTitle))
        {
            html.Add($"""<meta name="twitter:title" content="{Encode(Model.TwitterTitle)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.TwitterDescription))
        {
            html.Add($"""<meta name="twitter:description" content="{Encode(Model.TwitterDescription)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.TwitterImage))
        {
            html.Add($"""<meta name="twitter:image" content="{Encode(Model.TwitterImage)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.PrevUrl))
        {
            html.Add($"""<link rel="prev" href="{Encode(Model.PrevUrl)}" />""");
        }

        if (!string.IsNullOrWhiteSpace(Model.NextUrl))
        {
            html.Add($"""<link rel="next" href="{Encode(Model.NextUrl)}" />""");
        }

        output.Content.SetHtmlContent(string.Join(Environment.NewLine, html));
    }

    private static string Encode(string value) => WebUtility.HtmlEncode(value);
}