using Marketplace.Modules.Blog.Application.BlogPosts.Dtos;
using Marketplace.Modules.Blog.Application.Repositories;
using MediatR;

namespace Marketplace.Modules.Blog.Application.BlogPosts.Queries.GetBlogPostBySlug;

public sealed class GetBlogPostBySlugHandler : IRequestHandler<GetBlogPostBySlugQuery, BlogPostDto?>
{
    private readonly IBlogPostRepository _repository;

    public GetBlogPostBySlugHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogPostDto?> Handle(GetBlogPostBySlugQuery request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetBySlugAsync(request.Slug, cancellationToken);
        if (post is null) return null;

        return new BlogPostDto
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            Excerpt = post.Excerpt,
            Content = post.Content,
            CoverImageUrl = post.CoverImageUrl,
            IsPublished = post.IsPublished,
            PublishedAtUtc = post.PublishedAtUtc,
            CreatedAtUtc = post.CreatedAtUtc,
            UpdatedAtUtc = post.UpdatedAtUtc
        };
    }
}
