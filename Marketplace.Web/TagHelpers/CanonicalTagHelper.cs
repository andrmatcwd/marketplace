using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Net;

namespace Marketplace.Web.TagHelpers;

[HtmlTargetElement("canonical-link", TagStructure = TagStructure.WithoutEndTag)]
public sealed class CanonicalTagHelper : TagHelper
{
    public string? Href { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;

        if (string.IsNullOrWhiteSpace(Href))
        {
            output.Content.SetHtmlContent(string.Empty);
            return;
        }

        output.Content.SetHtmlContent($"""<link rel="canonical" href="{WebUtility.HtmlEncode(Href)}" />""");
    }
}