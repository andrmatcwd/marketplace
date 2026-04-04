using Marketplace.Modules.Listings.Domain.Enums;

namespace Marketplace.Modules.Listings.Domain.Entities;

public sealed class Listing
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public string SellerId { get; private set; }
    public ListingType Type { get; private set; }
    public ListingStatus Status { get; private set; }

    public Listing(Guid id, string title, string description, decimal price, string sellerId, ListingType type)
    {
        Id = id;
        Title = title;
        Description = description;
        Price = price;
        SellerId = sellerId;
        Type = type;
        Status = ListingStatus.Draft;
    }
}
