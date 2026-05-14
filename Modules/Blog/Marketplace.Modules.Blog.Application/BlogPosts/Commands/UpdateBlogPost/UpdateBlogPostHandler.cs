using Marketplace.Modules.Blog.Application.Repositories;
using MediatR;

namespace Marketplace.Modules.Blog.Application.BlogPosts.Commands.UpdateBlogPost;

public sealed class UpdateBlogPostHandler : IRequestHandler<UpdateBlogPostCommand, bool>
{
    private readonly IBlogPostRepository _repository;

    public UpdateBlogPostHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateBlogPostCommand request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (post is null) return false;

        var wasPublished = post.IsPublished;

        post.Title = request.Title;
        post.Slug = request.Slug;
        post.Excerpt = request.Excerpt;
        post.Content = request.Content;
        post.CoverImageUrl = request.CoverImageUrl;
        post.IsPublished = request.IsPublished;
        post.UpdatedAtUtc = DateTime.UtcNow;

        if (request.IsPublished && !wasPublished)
            post.PublishedAtUtc = DateTime.UtcNow;
        else if (!request.IsPublished)
            post.PublishedAtUtc = null;

        _repository.Update(post);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
