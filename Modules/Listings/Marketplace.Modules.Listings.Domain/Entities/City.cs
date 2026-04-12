using Marketplace.Modules.Listings.Domain.Contracts;
using Marketplace.Modules.Listings.Domain.Enums;

namespace Marketplace.Modules.Listings.Domain.Entities;

public class City : ISlugEntity
{
    public int Id { get; set; }

    public int RegionId { get; set; }
    public Region Region { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
