using System;
using Marketplace.Modules.Listings.Application.Repositories;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        this.reviewRepository = reviewRepository;
    }
}
