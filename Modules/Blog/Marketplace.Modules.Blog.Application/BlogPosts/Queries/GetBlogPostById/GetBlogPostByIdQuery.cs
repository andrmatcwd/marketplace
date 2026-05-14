using Marketplace.Modules.Blog.Application.BlogPosts.Dtos;
using MediatR;

namespace Marketplace.Modules.Blog.Application.BlogPosts.Queries.GetBlogPostById;

public sealed record GetBlogPostByIdQuery(int Id) : IRequest<BlogPostDto?>;
