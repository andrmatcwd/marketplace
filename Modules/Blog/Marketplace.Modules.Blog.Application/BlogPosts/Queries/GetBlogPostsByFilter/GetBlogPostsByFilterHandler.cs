using Marketplace.Modules.Blog.Application.BlogPosts.Dtos;
using Marketplace.Modules.Blog.Application.Common.Models;
using Marketplace.Modules.Blog.Application.Repositories;
using MediatR;

namespace Marketplace.Modules.Blog.Application.BlogPosts.Queries.GetBlogPostsByFilter;

public sealed class GetBlogPostsByFilterHandler
    : IRequestHandler<GetBlogPostsByFilterQuery, PagedResult<BlogPostDto>>
{
    private readonly IBlogPostRepository _repository;

    public GetBlogPostsByFilterHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<BlogPostDto>> Handle(
        GetBlogPostsByFilterQuery request,
        CancellationToken cancellationToken)
    {
        var (items, total) = await _repository.GetByFilterAsync(request.Filter, cancellationToken);

        return new PagedResult<BlogPostDto>
        {
            Items = items.Select(x => new BlogPostDto
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                Excerpt = x.Excerpt,
                Content = x.Content,
                CoverImageUrl = x.CoverImageUrl,
                IsPublished = x.IsPublished,
                PublishedAtUtc = x.PublishedAtUtc,
                CreatedAtUtc = x.CreatedAtUtc,
                UpdatedAtUtc = x.UpdatedAtUtc
            }).ToList(),
            Page = request.Filter.Page,
            PageSize = request.Filter.PageSize,
            TotalCount = total
        };
    }
}
