using System;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class ReviewerRepository
    : BaseRepository<Reviewer, int>, IReviewerRepository
{
    public ReviewerRepository(ListingsDbContext dbContext) : base(dbContext)
    {
    }
}
