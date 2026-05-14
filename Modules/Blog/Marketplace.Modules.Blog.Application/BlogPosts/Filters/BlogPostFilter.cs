using Marketplace.Modules.Blog.Application.Common.Models;

namespace Marketplace.Modules.Blog.Application.BlogPosts.Filters;

public sealed class BlogPostFilter : PaginationFilter
{
    public bool? IsPublished { get; init; }
}
