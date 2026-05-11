namespace Marketplace.Web.Helpers;

public static class MapUrlBuilder
{
    public static string? BuildGoogleMapsUrl(double? lat, double? lng, string? address)
    {
        if (lat.HasValue && lng.HasValue)
        {
            return $"https://www.google.com/maps?q={lat.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)},{lng.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
        }

        if (!string.IsNullOrWhiteSpace(address))
        {
            return $"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(address)}";
        }

        return null;
    }

    public static string? BuildDirectionsUrl(double? lat, double? lng, string? address)
    {
        if (lat.HasValue && lng.HasValue)
        {
            return $"https://www.google.com/maps/dir/?api=1&destination={lat.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)},{lng.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
        }

        if (!string.IsNullOrWhiteSpace(address))
        {
            return $"https://www.google.com/maps/dir/?api=1&destination={Uri.EscapeDataString(address)}";
        }

        return null;
    }
}