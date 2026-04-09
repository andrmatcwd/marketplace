using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Commands.EditCategory;

public sealed class EditCategoryHandler : IRequestHandler<EditCategoryCommand, int>
{
    public Task<int> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
