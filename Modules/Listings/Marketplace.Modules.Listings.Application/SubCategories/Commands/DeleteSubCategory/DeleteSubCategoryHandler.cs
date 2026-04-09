using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Commands.DeleteSubCategory;

public sealed class DeleteSubCategoryHandler : IRequestHandler<DeleteSubCategoryCommand, int>
{
    public Task<int> Handle(DeleteSubCategoryCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
