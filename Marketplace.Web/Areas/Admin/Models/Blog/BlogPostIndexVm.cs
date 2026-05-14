namespace Marketplace.Web.Areas.Admin.Models.Blog;

public class BlogPostIndexVm
{
    public string? Search { get; set; }
    public bool? IsPublished { get; set; }
    public IReadOnlyCollection<BlogPostListItemVm> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
}
