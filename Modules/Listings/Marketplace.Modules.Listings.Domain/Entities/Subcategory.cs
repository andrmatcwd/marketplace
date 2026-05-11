using System.ComponentModel.DataAnnotations;
using Marketplace.Modules.Listings.Domain.Contracts;

namespace Marketplace.Modules.Listings.Domain.Entities;

public class SubCategory : ISlugEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(140)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [MaxLength(255)]
    public string? Icon { get; set; }

    public bool IsPublished { get; set; }
    public int SortOrder { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
