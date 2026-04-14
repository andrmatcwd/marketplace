using Marketplace.Modules.Listings.Application.SubCategories.Commands.CreateSubCategory;
using Marketplace.Modules.Listings.Application.SubCategories.Commands.DeleteSubCategory;
using Marketplace.Modules.Listings.Application.SubCategories.Commands.EditSubCategory;
using Marketplace.Modules.Listings.Application.SubCategories.Filters;
using Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoryById;
using Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoriesByFilter;
using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Infrastructure.Persistence;
using Marketplace.Web.Areas.Admin.Models.SubCategories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
//[Authorize]
public class SubCategoriesController : Controller
{
    private readonly ListingsDbContext _db;
    private readonly ISender _sender;

    public SubCategoriesController(ListingsDbContext db, ISender sender)
    {
        _db = db;
        _sender = sender;
    }

    public async Task<IActionResult> Index(string? search, int? categoryId, CancellationToken cancellationToken)
    {
        var subCategories = await _sender.Send(new GetSubCategoriesByFilterQuery(new SubCategoryFilter
        {
            Search = search,
            //CategoryId = categoryId,
            Page = 1,
            PageSize = 3
        }), cancellationToken);

        ViewBag.Categories = await GetCategoriesSelectList(cancellationToken);

        var model = new SubCategoryIndexVm
        {
            Search = search,
            CategoryId = categoryId,
            Items = subCategories.Items
                .Select(x => new SubCategoryListItemVm
                {
                    Id = x.Id,
                    //CategoryId = x.CategoryId,
                    Name = x.Name,
                    Slug = x.Slug
                })
                .ToList()
        };

        return View(model);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        ViewBag.Categories = await GetCategoriesSelectList(cancellationToken);
        return View(new SubCategoryFormVm());
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubCategoryFormVm model, CancellationToken cancellationToken)
    {
        ViewBag.Categories = await GetCategoriesSelectList(cancellationToken);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _sender.Send(new CreateSubCategoryCommand(
            model.CategoryId,
            model.Name,
            model.Description), cancellationToken);

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
            //CategoryId = subCategory.CategoryId,
            Name = subCategory.Name,
            Slug = subCategory.Slug,
            Description = subCategory.Description
        });
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SubCategoryFormVm model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();

        ViewBag.Categories = await GetCategoriesSelectList(cancellationToken);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _sender.Send(new EditSubCategoryCommand(
            id,
            model.CategoryId,
            model.Name,
            model.Slug,
            model.Description
        ), cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var subCategory = await _sender.Send(new GetSubCategoryByIdQuery(id), cancellationToken);
        if (subCategory is null) return NotFound();

        return View(new SubCategoryListItemVm
        {
            Id = subCategory.Id,
            //CategoryId = subCategory.CategoryId,
            Name = subCategory.Name,
            Slug = subCategory.Slug,
        });
    }

    [HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteSubCategoryCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> GetCategoriesSelectList(CancellationToken cancellationToken)
    {
        return await _db.Categories
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToListAsync(cancellationToken);
    }
}