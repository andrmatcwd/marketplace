using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Models.Api;

public sealed class ListingContactRequestDto
{
    [Required]
    public int ListingId { get; set; }

    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [StringLength(2000, MinimumLength = 5)]
    public string Message { get; set; } = string.Empty;
}