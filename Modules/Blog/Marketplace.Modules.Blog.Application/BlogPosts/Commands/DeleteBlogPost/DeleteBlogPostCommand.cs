using MediatR;

namespace Marketplace.Modules.Blog.Application.BlogPosts.Commands.DeleteBlogPost;

public sealed record DeleteBlogPostCommand(int Id) : IRequest<bool>;
