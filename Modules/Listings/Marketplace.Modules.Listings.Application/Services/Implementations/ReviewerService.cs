using System;
using Marketplace.Modules.Listings.Application.Repositories;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class ReviewerService : IReviewerService
{
    private readonly IReviewerRepository reviewerRepository;

    public ReviewerService(IReviewerRepository reviewerRepository)
    {
        this.reviewerRepository = reviewerRepository;
    }
}
