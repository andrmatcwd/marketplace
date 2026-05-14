using MediatR;

namespace Marketplace.Modules.Blog.Application.BlogPosts.Commands.CreateBlogPost;

public sealed record CreateBlogPostCommand(
    string Title,
    string Slug,
    string Excerpt,
    string Content,
    string? CoverImageUrl,
    bool IsPublished
) : IRequest<int>;
