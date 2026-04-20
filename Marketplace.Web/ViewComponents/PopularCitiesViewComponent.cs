using Marketplace.Web.Services.Home;
using Marketplace.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.ViewComponents;

public sealed class PopularCitiesViewComponent : ViewComponent
{
    private readonly IHomeService _homeService;

    public PopularCitiesViewComponent(IHomeService homeService)
    {
        _homeService = homeService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? culture = null, CancellationToken cancellationToken = default)
    {
        culture ??= CultureHelper.Current();

        var vm = await _homeService.GetHomePageAsync(culture, cancellationToken);
        return View(vm.PopularCitiesSection.Items);
    }
}