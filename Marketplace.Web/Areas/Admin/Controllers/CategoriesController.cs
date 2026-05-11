using Marketplace.Modules.Listings.Application.Categories.Commands.CreateCategory;
using Marketplace.Modules.Listings.Application.Categories.Commands.DeleteCategory;
using Marketplace.Modules.Listings.Application.Categories.Commands.EditCategory;
using Marketplace.Modules.Listings.Application.Categories.Filters;
using Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoriesByFilter;
using Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoryById;
using Marketplace.Web.Areas.Admin.Models.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoriesController : Controller
{
    private readonly ISender _sender;

    public CategoriesController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Index(string? search, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCategoriesByFilterQuery(new CategoryFilter
        {
            Search = search,
            PageSize = 25
        }), cancellationToken);

        return View(new CategoryIndexVm
        {
            Search = search,
            Items = result.Items
                .OrderBy(x => x.Name)
                .Select(x => new CategoryListItemVm
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    SubCategoriesCount = x.SubCategoriesCount,
                    ListingsCount = x.ListingsCount
                })
                .ToList()
        });
    }

    public IActionResult Create() => View(new CategoryFormVm());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryFormVm model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new CreateCategoryCommand(
            model.Name,
            model.Slug,
            model.Description,
            model.Icon,
            model.IsPublished,
            model.SortOrder), cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var category = await _sender.Send(new GetCategoryByIdQuery(id), cancellationToken);
        if (category is null) return NotFound();

        return View(new CategoryFormVm
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            Icon = category.Icon
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryFormVm model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new EditCategoryCommand(
            id,
            model.Name,
            model.Slug,
            model.Description,
            model.Icon,
            model.IsPublished,
            model.SortOrder), cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var category = await _sender.Send(new GetCategoryByIdQuery(id), cancellationToken);
        if (category is null) return NotFound();

        return View(new CategoryListItemVm
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            SubCategoriesCount = category.SubCategoriesCount,
            ListingsCount = category.ListingsCount
        });
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteCategoryCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
