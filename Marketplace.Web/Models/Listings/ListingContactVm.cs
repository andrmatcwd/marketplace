namespace Marketplace.Web.Models.Listings;

public sealed class ListingContactVm
{
    public string? ContactName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }

    public bool HasPhone => !string.IsNullOrWhiteSpace(Phone);
    public bool HasEmail => !string.IsNullOrWhiteSpace(Email);
    public bool HasWebsite => !string.IsNullOrWhiteSpace(Website);

    public bool HasAny =>
        !string.IsNullOrWhiteSpace(ContactName) ||
        !string.IsNullOrWhiteSpace(Phone) ||
        !string.IsNullOrWhiteSpace(Email) ||
        !string.IsNullOrWhiteSpace(Website);
}