using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Models.Listings.Forms;

public sealed class CreateListingReviewVm
{
    [Required]
    public int ListingId { get; set; }

    [Required(ErrorMessage = "Вкажіть ім’я")]
    [StringLength(120)]
    public string AuthorName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Некоректний email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Оберіть оцінку")]
    [Range(1, 5)]
    public int? Rating { get; set; }

    [Required(ErrorMessage = "Введіть текст відгуку")]
    [StringLength(2000, MinimumLength = 5)]
    public string Text { get; set; } = string.Empty;
}