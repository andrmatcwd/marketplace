namespace Marketplace.Modules.Listings.Application.Listings.Dtos;

public class ListingDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string LocationName { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; }
}
