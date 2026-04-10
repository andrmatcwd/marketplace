using System;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Seo;

public static class SeoExtensions
{
    public static void SetSeo(this Controller controller, PageSeoData seo)
    {
        controller.ViewData["Seo"] = seo;
    }
}
