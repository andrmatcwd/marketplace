using MediatR;

namespace Marketplace.Modules.Blog.Application.BlogPosts.Commands.UpdateBlogPost;

public sealed record UpdateBlogPostCommand(
    int Id,
    string Title,
    string Slug,
    string Excerpt,
    string Content,
    string? CoverImageUrl,
    bool IsPublished
) : IRequest<bool>;
