using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Seo;
using Marketplace.Web.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class FavoritesController : Controller
{
    private readonly ISender _sender;
    private readonly ICatalogVmMapper _mapper;

    public FavoritesController(ISender sender, ICatalogVmMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/favorites")]
    public async Task<IActionResult> Index(string culture, CancellationToken cancellationToken = default)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);
        var isUk = culture == "uk";

        var ids = ParseFavoriteCookie();

        IReadOnlyCollection<ListingCardVm> listings = Array.Empty<ListingCardVm>();
        if (ids.Count > 0)
        {
            var dtos = await _sender.Send(new GetListingsByIdsQuery(ids), cancellationToken);
            listings = dtos.Select(dto => _mapper.MapListingCard(dto, culture)).ToList();
        }

        ViewData["Culture"]  = culture;
        ViewData["Listings"] = listings;
        ViewData["Seo"] = new PageSeoData
        {
            Title       = isUk ? "Збережені оголошення" : "Сохранённые объявления",
            Description = isUk
                ? "Оголошення та компанії, які ви зберегли на Marketplace."
                : "Объявления и компании, которые вы сохранили на Marketplace.",
            Robots = "noindex, nofollow"
        };

        return View();
    }

    private IReadOnlyList<int> ParseFavoriteCookie()
    {
        var cookie = Request.Cookies["mp_favorites"];
        if (string.IsNullOrWhiteSpace(cookie)) return Array.Empty<int>();

        return cookie
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => int.TryParse(s.Trim(), out var n) ? n : 0)
            .Where(n => n > 0)
            .Distinct()
            .Take(50)
            .ToList();
    }
}
