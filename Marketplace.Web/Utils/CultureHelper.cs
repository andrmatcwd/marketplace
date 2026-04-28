using System.Globalization;

namespace Marketplace.Web.Utils;

public static class CultureHelper
{
    public const string DefaultRouteCulture = "uk";
    public const string DefaultFullCulture = "uk-UA";

    public static string NormalizeRouteCulture(string? culture)
    {
        if (string.IsNullOrWhiteSpace(culture))
        {
            return DefaultRouteCulture;
        }

        culture = culture.Trim().ToLowerInvariant();

        return culture switch
        {
            "uk" => "uk",
            "ru" => "ru",
            _ => DefaultRouteCulture
        };
    }

    public static string ToFullCulture(string? routeCulture)
    {
        return NormalizeRouteCulture(routeCulture) switch
        {
            "uk" => "uk-UA",
            "ru" => "ru-RU",
            _ => DefaultFullCulture
        };
    }

    public static string ToRouteCulture(string? fullCulture)
    {
        if (string.IsNullOrWhiteSpace(fullCulture))
        {
            return DefaultRouteCulture;
        }

        fullCulture = fullCulture.Trim().ToLowerInvariant();

        return fullCulture switch
        {
            "uk" or "uk-ua" => "uk",
            "ru" or "ru-ru" => "ru",
            _ => DefaultRouteCulture
        };
    }

    public static string Current()
    {
        return ToRouteCulture(CultureInfo.CurrentUICulture.Name);
    }

    public static bool IsUk()
    {
        return Current() == "uk";
    }

    public static bool IsRu()
    {
        return Current() == "ru";
    }
}