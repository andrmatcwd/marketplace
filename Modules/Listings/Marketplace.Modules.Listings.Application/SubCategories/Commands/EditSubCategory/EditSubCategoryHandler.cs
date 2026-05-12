using Marketplace.Modules.Listings.Application.Services;
using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Commands.EditSubCategory;

public sealed class EditSubCategoryHandler : IRequestHandler<EditSubCategoryCommand, Unit>
{
    private readonly ISubCategoryService _subCategoryService;

    public EditSubCategoryHandler(ISubCategoryService subCategoryService)
    {
        _subCategoryService = subCategoryService;
    }

    public async Task<Unit> Handle(EditSubCategoryCommand request, CancellationToken cancellationToken)
    {
        await _subCategoryService.EditAsync(request, cancellationToken);
        return Unit.Value;
    }
}
