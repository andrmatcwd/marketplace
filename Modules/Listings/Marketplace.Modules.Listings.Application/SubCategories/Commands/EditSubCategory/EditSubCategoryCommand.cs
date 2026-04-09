using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Commands.EditSubCategory;

public sealed record EditSubCategoryCommand(int Id, int CategoryId, string Name, string Slug, string? Description) : IRequest<int>;
