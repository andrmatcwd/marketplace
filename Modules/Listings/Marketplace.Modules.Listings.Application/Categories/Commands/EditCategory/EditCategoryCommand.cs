using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Commands.EditCategory;

public sealed record EditCategoryCommand(
    int Id,
    string Name,
    string? Slug,
    string? Description,
    string? Icon,
    bool IsPublished,
    int SortOrder
) : IRequest<Unit>;
