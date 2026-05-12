using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Commands.EditCategory;

public sealed class EditCategoryHandler
    : IRequestHandler<EditCategoryCommand, Unit>
{
    private readonly ICategoryService _categoryRepository;

    public EditCategoryHandler(ICategoryService categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Unit> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        await _categoryRepository.EditAsync(request, cancellationToken);
        return Unit.Value;
    }
}
