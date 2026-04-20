using System.Text.RegularExpressions;

namespace Marketplace.Web.Utils;

public static class TextHelper
{
    public static string TrimToLength(string? text, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        var plain = Regex.Replace(text, "<.*?>", string.Empty).Trim();

        if (plain.Length <= maxLength)
        {
            return plain;
        }

        return plain[..maxLength].Trim() + "...";
    }
}