using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Models.Api;

public sealed class BusinessInquiryDto
{
    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(200)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? CompanyName { get; set; }

    [Required]
    [StringLength(2000, MinimumLength = 5)]
    public string Message { get; set; } = string.Empty;
}
