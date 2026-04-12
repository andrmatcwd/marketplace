using Marketplace.Modules.Listings.Application.Categories.Commands.CreateCategory;
using Marketplace.Modules.Listings.Application.Categories.Commands.DeleteCategory;
using Marketplace.Modules.Listings.Application.Categories.Commands.EditCategory;
using Marketplace.Modules.Listings.Application.Categories.Filters;
using Marketplace.Modules.Listings.Application.Categories.Queries.GetById;
using Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoriesByFilter;
using Marketplace.Web.Areas.Admin.Models.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
//[Authorize]
public class CategoriesController : Controller
{
    private readonly ISender _sender;

    public CategoriesController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Index(string? search, CancellationToken cancellationToken)
    {
        var categories = await _sender.Send(new GetCategoriesByFilterQuery(new CategoryFilter
        {
            Search = search,
            Page = 1,
            PageSize = int.MaxValue
        }), cancellationToken);

        var model = new CategoryIndexVm
        {
            Search = search,
            Items = categories.Items
                .OrderBy(x => x.Name)
                .Select(x => new CategoryListItemVm
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                }).ToList()
        };

        return View(model);
    }

    public IActionResult Create()
    {
        return View(new CategoryFormVm());
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryFormVm model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _sender.Send(new CreateCategoryCommand(
            model.Name,
            model.Slug,
            model.Description,
            model.Icon), cancellationToken);

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
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryFormVm model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _sender.Send(new EditCategoryCommand(
            model.Id.Value,
            model.Name,
            model.Slug,
            model.Description,
            model.Icon), cancellationToken);
            
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
            //SubCategoriesCount = category.SubCategories.Count,
            //ListingsCount = category.Listings.Count
        });
    }

    [HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {

        await _sender.Send(new DeleteCategoryCommand(id), cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}