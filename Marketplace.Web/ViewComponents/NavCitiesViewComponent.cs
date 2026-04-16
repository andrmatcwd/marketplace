using Marketplace.Web.Data;
using Marketplace.Web.Models.Shared;
using Marketplace.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.ViewComponents;

public sealed class NavCitiesViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _dbContext;

    public NavCitiesViewComponent(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var culture = CultureHelper.Current();

        var cities = await _dbContext.Cities
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new NavCityOptionVm
            {
                Name = x.Name,
                Slug = x.Slug,
                Url = $"/{culture}/{x.Slug}"
            })
            .ToListAsync();

        return View(cities);
    }
}