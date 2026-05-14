using Marketplace.Modules.Blog.Application.BlogPosts.Dtos;
using Marketplace.Modules.Blog.Application.Repositories;
using MediatR;

namespace Marketplace.Modules.Blog.Application.BlogPosts.Queries.GetBlogPostById;

public sealed class GetBlogPostByIdHandler : IRequestHandler<GetBlogPostByIdQuery, BlogPostDto?>
{
    private readonly IBlogPostRepository _repository;

    public GetBlogPostByIdHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogPostDto?> Handle(GetBlogPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetByIdAsync(request.Id, cancellationToken);
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
