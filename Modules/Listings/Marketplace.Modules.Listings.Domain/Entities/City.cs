using System.ComponentModel.DataAnnotations;
using Marketplace.Modules.Listings.Domain.Contracts;

namespace Marketplace.Modules.Listings.Domain.Entities;

public class City : ISlugEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Slug { get; set; } = string.Empty;

    public int RegionId { get; set; }
    public Region Region { get; set; } = null!;

    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
