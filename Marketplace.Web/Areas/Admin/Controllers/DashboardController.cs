using Marketplace.Modules.Listings.Domain.Enums;
using Marketplace.Modules.Listings.Infrastructure.Persistence;
using Marketplace.Web.Areas.Admin.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
//[Authorize]
public class DashboardController : Controller
{
    private readonly ListingsDbContext _db;

    public DashboardController(ListingsDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = new DashboardVm
        {
            // RegionsCount = await _db.Regions.CountAsync(cancellationToken),
            // CitiesCount = await _db.Cities.CountAsync(cancellationToken),
            // CategoriesCount = await _db.Categories.CountAsync(cancellationToken),
            // SubCategoriesCount = await _db.SubCategories.CountAsync(cancellationToken),
            // ListingsCount = await _db.Listings.CountAsync(cancellationToken),
            // RecentListings = await _db.Listings
            //     .AsNoTracking()
            //     .Include(x => x.Category)
            //     .Include(x => x.SubCategory)
            //     .Include(x => x.City)
            //     .OrderByDescending(x => x.CreatedAtUtc)
            //     .Take(10)
            //     .Select(x => new RecentListingVm
            //     {
            //         Id = x.Id,
            //         Title = x.Title,
            //         Category = x.Category.Name,
            //         SubCategory = x.SubCategory.Name,
            //         City = x.City.Name,
            //         CreatedUtc = x.CreatedAtUtc
            //     })
            //     .ToListAsync(cancellationToken)
        };

        return View(model);
    }
}