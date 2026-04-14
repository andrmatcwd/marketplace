using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Marketplace.Web.Seo;

public static class SeoExtensions
{
    private const string SeoViewDataKey = "Seo";

    public static void SetSeo(this Controller controller, PageSeoData seo)
    {
        controller.ViewData[SeoViewDataKey] = seo;
    }

    public static PageSeoData? GetSeo(this ViewDataDictionary viewData)
    {
        return viewData[SeoViewDataKey] as PageSeoData;
    }
}