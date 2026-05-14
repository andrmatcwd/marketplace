using Marketplace.Modules.Blog.Application.BlogPosts.Commands.CreateBlogPost;
using Marketplace.Modules.Blog.Application.BlogPosts.Commands.DeleteBlogPost;
using Marketplace.Modules.Blog.Application.BlogPosts.Commands.UpdateBlogPost;
using Marketplace.Modules.Blog.Application.BlogPosts.Filters;
using Marketplace.Modules.Blog.Application.BlogPosts.Queries.GetBlogPostById;
using Marketplace.Modules.Blog.Application.BlogPosts.Queries.GetBlogPostsByFilter;
using Marketplace.Web.Areas.Admin.Models.Blog;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class BlogController : Controller
{
    private readonly ISender _sender;
    private const int PageSize = 20;

    public BlogController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Index(string? search, bool? isPublished, int page = 1, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetBlogPostsByFilterQuery(new BlogPostFilter
        {
            Search = search,
            IsPublished = isPublished,
            Page = page,
            PageSize = PageSize
        }), cancellationToken);

        return View(new BlogPostIndexVm
        {
            Search = search,
            IsPublished = isPublished,
            Page = page,
            TotalPages = result.TotalPages,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(x => new BlogPostListItemVm
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                IsPublished = x.IsPublished,
                CreatedAtUtc = x.CreatedAtUtc,
                UpdatedAtUtc = x.UpdatedAtUtc
            }).ToList()
        });
    }

    public IActionResult Create() => View(new BlogPostFormVm());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BlogPostFormVm model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new CreateBlogPostCommand(
            model.Title,
            model.Slug,
            model.Excerpt,
            model.Content,
            model.CoverImageUrl,
            model.IsPublished), cancellationToken);

        TempData["Success"] = "Пост успішно створено.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var post = await _sender.Send(new GetBlogPostByIdQuery(id), cancellationToken);
        if (post is null) return NotFound();

        return View(new BlogPostFormVm
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            Excerpt = post.Excerpt,
            Content = post.Content,
            CoverImageUrl = post.CoverImageUrl,
            IsPublished = post.IsPublished
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BlogPostFormVm model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new UpdateBlogPostCommand(
            id,
            model.Title,
            model.Slug,
            model.Excerpt,
            model.Content,
            model.CoverImageUrl,
            model.IsPublished), cancellationToken);

        TempData["Success"] = "Пост успішно оновлено.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var post = await _sender.Send(new GetBlogPostByIdQuery(id), cancellationToken);
        if (post is null) return NotFound();

        return View(new BlogPostListItemVm
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            IsPublished = post.IsPublished,
            CreatedAtUtc = post.CreatedAtUtc,
            UpdatedAtUtc = post.UpdatedAtUtc
        });
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteBlogPostCommand(id), cancellationToken);
        TempData["Success"] = "Пост видалено.";
        return RedirectToAction(nameof(Index));
    }
}
