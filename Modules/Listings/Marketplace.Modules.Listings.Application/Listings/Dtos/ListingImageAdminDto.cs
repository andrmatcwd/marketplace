namespace Marketplace.Modules.Listings.Application.Listings.Dtos;

public sealed class ListingImageAdminDto
{
    public int Id { get; init; }
    public string Url { get; init; } = string.Empty;
    public string? Alt { get; init; }
    public bool IsPrimary { get; init; }
    public int SortOrder { get; init; }
}
