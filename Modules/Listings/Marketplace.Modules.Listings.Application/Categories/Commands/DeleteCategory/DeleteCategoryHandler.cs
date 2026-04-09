using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, int>
{
    public Task<int> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
