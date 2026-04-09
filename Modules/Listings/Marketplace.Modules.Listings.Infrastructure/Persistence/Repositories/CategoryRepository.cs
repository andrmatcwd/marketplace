using System;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class CategoryRepository
    : BaseRepository<Category, int>, ICategoryRepository
{
    public CategoryRepository(ListingsDbContext dbContext) : base(dbContext)
    {
    }
}
