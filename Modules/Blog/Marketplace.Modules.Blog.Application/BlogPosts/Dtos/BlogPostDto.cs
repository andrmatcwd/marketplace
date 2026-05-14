namespace Marketplace.Modules.Blog.Application.BlogPosts.Dtos;

public class BlogPostDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Excerpt { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public string? CoverImageUrl { get; init; }
    public bool IsPublished { get; init; }
    public DateTime? PublishedAtUtc { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
}
