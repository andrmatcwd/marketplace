using Marketplace.Modules.Blog.Application.BlogPosts.Dtos;
using MediatR;

namespace Marketplace.Modules.Blog.Application.BlogPosts.Queries.GetBlogPostBySlug;

public sealed record GetBlogPostBySlugQuery(string Slug) : IRequest<BlogPostDto?>;
