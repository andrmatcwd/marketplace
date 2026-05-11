using System.ComponentModel.DataAnnotations;
using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Web.Areas.Admin.Models.Listings;

public class ListingFormVm
{
    public int? Id { get; set; }

    [Required]
    [Display(Name = "Назва")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Slug (авто якщо порожній)")]
    public string? Slug { get; set; }

    [Display(Name = "Короткий опис")]
    public string? ShortDescription { get; set; }

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Display(Name = "Телефон")]
    public string? Phone { get; set; }

    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Display(Name = "Сайт")]
    public string? Website { get; set; }

    [Display(Name = "Категорія")]
    public int CategoryId { get; set; }

    [Display(Name = "Підкатегорія")]
    public int SubCategoryId { get; set; }

    [Display(Name = "Місто")]
    public int CityId { get; set; }

    [Display(Name = "Адреса")]
    public string? Address { get; set; }

    [Display(Name = "Latitude")]
    public double? Latitude { get; set; }

    [Display(Name = "Longitude")]
    public double? Longitude { get; set; }

    [Display(Name = "Seller Id")]
    public string? SellerId { get; set; }

    [Display(Name = "Статус")]
    public ListingStatus Status { get; set; } = ListingStatus.Active;

    [Display(Name = "Тип підписки")]
    public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Free;
}
