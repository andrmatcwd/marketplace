using System;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class SubCategoryRepository
    : BaseRepository<SubCategory, int>, ISubCategoryRepository
{
    public SubCategoryRepository(ListingsDbContext dbContext) : base(dbContext)
    {
    }
}
