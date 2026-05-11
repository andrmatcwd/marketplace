using Marketplace.Modules.Listings.Application.Categories.Filters;
using Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoriesByFilter;
using Marketplace.Modules.Listings.Application.Cities.Filters;
using Marketplace.Modules.Listings.Application.Cities.Queries.GetCitiesByFilter;
using Marketplace.Modules.Listings.Application.Listings.Filters;
using Marketplace.Modules.Listings.Application.Listings.Queries.GetListingsByFilter;
using Marketplace.Modules.Listings.Application.SubCategories.Filters;
using Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoriesByFilter;
using Marketplace.Web.Areas.Admin.Models.Dashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly ISender _sender;

    public DashboardController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var citiesTask = _sender.Send(new GetCitiesByFilterQuery(new CityFilter { PageSize = 1 }), cancellationToken);
        var categoriesTask = _sender.Send(new GetCategoriesByFilterQuery(new CategoryFilter { PageSize = 1 }), cancellationToken);
        var subCategoriesTask = _sender.Send(new GetSubCategoriesByFilterQuery(new SubCategoryFilter { PageSize = 1 }), cancellationToken);
        var listingsTask = _sender.Send(new GetListingsByFilterQuery(new ListingFilter { PageSize = 1 }), cancellationToken);
        var recentListingsTask = _sender.Send(new GetListingsByFilterQuery(new ListingFilter { PageSize = 10, SortBy = "date", SortDescending = true }), cancellationToken);

        await Task.WhenAll(citiesTask, categoriesTask, subCategoriesTask, listingsTask, recentListingsTask);

        var model = new DashboardVm
        {
            CitiesCount = citiesTask.Result.TotalCount,
            CategoriesCount = categoriesTask.Result.TotalCount,
            SubCategoriesCount = subCategoriesTask.Result.TotalCount,
            ListingsCount = listingsTask.Result.TotalCount,
            RecentListings = recentListingsTask.Result.Items
                .Select(x => new RecentListingVm
                {
                    Id = x.Id,
                    Title = x.Title,
                    Category = x.CategoryName,
                    SubCategory = x.SubCategoryName,
                    City = x.CityName,
                    CreatedUtc = x.CreatedUtc
                })
                .ToList()
        };

        return View(model);
    }
}
