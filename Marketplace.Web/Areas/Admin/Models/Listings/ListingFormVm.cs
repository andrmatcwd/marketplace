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

    [Required]
    [Display(Name = "Slug")]
    public string Slug { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Опис")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Категорія")]
    public int CategoryId { get; set; }

    [Display(Name = "Підкатегорія")]
    public int SubCategoryId { get; set; }

    [Display(Name = "Місто")]
    public int LocationId { get; set; }

    [Display(Name = "Ціна")]
    public decimal Price { get; set; }

    [Required]
    [Display(Name = "Валюта")]
    public string Currency { get; set; } = "USD";

    [Required]
    [Display(Name = "Seller Id")]
    public string SellerId { get; set; } = string.Empty;

    [Display(Name = "Адреса")]
    public string? AddressLine { get; set; }

    [Display(Name = "Latitude")]
    public double? Latitude { get; set; }

    [Display(Name = "Longitude")]
    public double? Longitude { get; set; }

    [Display(Name = "Статус")]
    public ListingStatus Status { get; set; } = ListingStatus.Active;

    [Display(Name = "Тип підписки")]
    public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Free;
}