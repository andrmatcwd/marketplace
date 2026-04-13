using System.ComponentModel.DataAnnotations;

namespace Marketplace.Modules.Listings.Domain.Entities;

public class Region
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Slug { get; set; } = string.Empty;

    public ICollection<City> Cities { get; set; } = new List<City>();
}
