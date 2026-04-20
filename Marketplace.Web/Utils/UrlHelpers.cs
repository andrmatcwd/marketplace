namespace Marketplace.Web.Utils;

public static class UrlHelpers
{
    public static string Combine(params string?[] segments)
    {
        var cleaned = segments
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!.Trim('/'));

        return "/" + string.Join("/", cleaned);
    }
}