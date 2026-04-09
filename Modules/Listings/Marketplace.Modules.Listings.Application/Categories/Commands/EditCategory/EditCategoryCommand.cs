using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Commands.EditCategory;

public sealed record EditCategoryCommand(int Id, string Name, string Slug, string? Description, string? Icon) : IRequest<int>;
