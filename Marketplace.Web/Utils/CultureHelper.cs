using System.Globalization;

namespace Marketplace.Web.Utils;

public static class CultureHelper
{
    public static string Normalize(string? culture)
    {
        if (string.IsNullOrWhiteSpace(culture))
        {
            return "uk";
        }

        culture = culture.Trim().ToLowerInvariant();

        return culture switch
        {
            "uk" => "uk",
            "en" => "en",
            _ => "uk"
        };
    }

    public static string Current()
    {
        return Normalize(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
    }
}