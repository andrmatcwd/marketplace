using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Areas.Admin.Models.Blog;

public class BlogPostFormVm
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Заголовок обов'язковий")]
    [StringLength(300, ErrorMessage = "Максимум 300 символів")]
    [Display(Name = "Заголовок")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Slug обов'язковий")]
    [StringLength(300, ErrorMessage = "Максимум 300 символів")]
    [Display(Name = "Slug")]
    public string Slug { get; set; } = string.Empty;

    [Required(ErrorMessage = "Короткий опис обов'язковий")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Від 10 до 1000 символів")]
    [Display(Name = "Короткий опис")]
    public string Excerpt { get; set; } = string.Empty;

    [Required(ErrorMessage = "Зміст обов'язковий")]
    [MinLength(10, ErrorMessage = "Мінімум 10 символів")]
    [Display(Name = "Зміст")]
    public string Content { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Максимум 2000 символів")]
    [Display(Name = "URL зображення обкладинки")]
    public string? CoverImageUrl { get; set; }

    [Display(Name = "Опубліковано")]
    public bool IsPublished { get; set; }
}
