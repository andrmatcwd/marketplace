using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Commands.EditSubCategory;

public sealed class EditSubCategoryHandler : IRequestHandler<EditSubCategoryCommand, int>
{
    public Task<int> Handle(EditSubCategoryCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
