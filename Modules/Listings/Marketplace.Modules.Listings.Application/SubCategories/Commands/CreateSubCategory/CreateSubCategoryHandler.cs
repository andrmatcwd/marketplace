using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Commands.CreateSubCategory;

public sealed class CreateSubCategoryHandler : IRequestHandler<CreateSubCategoryCommand, Unit>
{
    private readonly ISubCategoryService _subCategoryService;

    public CreateSubCategoryHandler(ISubCategoryService subCategoryService)
    {
        _subCategoryService = subCategoryService;
    }

    public async Task<Unit> Handle(CreateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        await _subCategoryService.AddAsync(request, cancellationToken);

        return Unit.Value;
    }
}
