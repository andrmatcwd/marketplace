using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Commands.CreateSubCategory;

public sealed record CreateSubCategoryCommand(
    int CategoryId,
    string Name,
    string? Slug,
    string? Description,
    string? Icon,
    bool IsPublished,
    int SortOrder
) : IRequest<Unit>;
