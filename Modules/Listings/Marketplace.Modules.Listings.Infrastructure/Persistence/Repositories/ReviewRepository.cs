using System;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class ReviewRepository
    : BaseRepository<Review, int>, IReviewRepository
{
    public ReviewRepository(ListingsDbContext dbContext) : base(dbContext)
    {
    }
}
