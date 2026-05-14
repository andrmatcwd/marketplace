using Marketplace.Modules.Blog.Application.Repositories;
using MediatR;

namespace Marketplace.Modules.Blog.Application.BlogPosts.Commands.DeleteBlogPost;

public sealed class DeleteBlogPostHandler : IRequestHandler<DeleteBlogPostCommand, bool>
{
    private readonly IBlogPostRepository _repository;

    public DeleteBlogPostHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteBlogPostCommand request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (post is null) return false;

        _repository.Remove(post);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
