using Marketplace.Modules.Listings.Application.Categories.Filters;
using Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoriesByFilter;
using Marketplace.Modules.Listings.Application.SubCategories.Commands.CreateSubCategory;
using Marketplace.Modules.Listings.Application.SubCategories.Commands.DeleteSubCategory;
using Marketplace.Modules.Listings.Application.SubCategories.Commands.EditSubCategory;
using Marketplace.Modules.Listings.Application.SubCategories.Filters;
using Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoriesByFilter;
using Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoryById;
using Marketplace.Web.Areas.Admin.Models.SubCategories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SubCategoriesController : Controller
{
    private readonly ISender _sender;

    public SubCategoriesController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Index(string? search, int? categoryId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetSubCategoriesByFilterQuery(new SubCategoryFilter
        {
            Search = search,
            PageSize = 25
        }), cancellationToken);

        ViewBag.Categories = await GetCategoriesSelectList(cancellationToken);

        return View(new SubCategoryIndexVm
        {
            Search = search,
            CategoryId = categoryId,
            Items = result.Items
                .Select(x => new SubCategoryListItemVm
                {
                    Id = x.Id,
                    CategoryId = 0,
                    CategoryName = x.CategoryName,
                    Name = x.Name,
                    Slug = x.Slug,
                    ListingsCount = x.ListingsCount
                })
                .ToList()
        });
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        ViewBag.Categories = await GetCategoriesSelectList(cancellationToken);
        return View(new SubCategoryFormVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubCategoryFormVm model, CancellationToken cancellationToken)
    {
        ViewBag.Categories = await GetCategoriesSelectList(cancellationToken);
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new CreateSubCategoryCommand(
            model.CategoryId,
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
        var subCategory = await _sender.Send(new GetSubCategoryByIdQuery(id), cancellationToken);
        if (subCategory is null) return NotFound();

        ViewBag.Categories = await GetCategoriesSelectList(cancellationToken);

        return View(new SubCategoryFormVm
        {
            Id = subCategory.Id,
            Name = subCategory.Name,
            Slug = subCategory.Slug,
            Description = subCategory.Description,
            Icon = subCategory.Icon
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SubCategoryFormVm model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();

        ViewBag.Categories = await GetCategoriesSelectList(cancellationToken);
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new EditSubCategoryCommand(
            id,
            model.CategoryId,
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
        var subCategory = await _sender.Send(new GetSubCategoryByIdQuery(id), cancellationToken);
        if (subCategory is null) return NotFound();

        return View(new SubCategoryListItemVm
        {
            Id = subCategory.Id,
            Name = subCategory.Name,
            Slug = subCategory.Slug,
            CategoryName = subCategory.CategoryName
        });
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteSubCategoryCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> GetCategoriesSelectList(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCategoriesByFilterQuery(new CategoryFilter { PageSize = 100 }), cancellationToken);
        return result.Items
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
            .ToList();
    }
}
