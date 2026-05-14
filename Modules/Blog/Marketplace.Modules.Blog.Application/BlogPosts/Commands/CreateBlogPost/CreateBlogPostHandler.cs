using Marketplace.Modules.Blog.Application.Repositories;
using Marketplace.Modules.Blog.Domain.Entities;
using MediatR;

namespace Marketplace.Modules.Blog.Application.BlogPosts.Commands.CreateBlogPost;

public sealed class CreateBlogPostHandler : IRequestHandler<CreateBlogPostCommand, int>
{
    private readonly IBlogPostRepository _repository;

    public CreateBlogPostHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateBlogPostCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var post = new BlogPost
        {
            Title = request.Title,
            Slug = request.Slug,
            Excerpt = request.Excerpt,
            Content = request.Content,
            CoverImageUrl = request.CoverImageUrl,
            IsPublished = request.IsPublished,
            PublishedAtUtc = request.IsPublished ? now : null,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        await _repository.AddAsync(post, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return post.Id;
    }
}
