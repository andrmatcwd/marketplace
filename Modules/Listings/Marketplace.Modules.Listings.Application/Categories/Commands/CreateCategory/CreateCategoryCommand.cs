using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(string Name, string Slug, string? Description, string? Icon) : IRequest<int>;
