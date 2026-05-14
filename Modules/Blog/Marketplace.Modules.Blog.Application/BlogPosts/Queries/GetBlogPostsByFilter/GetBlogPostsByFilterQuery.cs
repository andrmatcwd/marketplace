using Marketplace.Modules.Blog.Application.BlogPosts.Dtos;
using Marketplace.Modules.Blog.Application.BlogPosts.Filters;
using Marketplace.Modules.Blog.Application.Common.Models;
using MediatR;

namespace Marketplace.Modules.Blog.Application.BlogPosts.Queries.GetBlogPostsByFilter;

public sealed record GetBlogPostsByFilterQuery(BlogPostFilter Filter)
    : IRequest<PagedResult<BlogPostDto>>;
