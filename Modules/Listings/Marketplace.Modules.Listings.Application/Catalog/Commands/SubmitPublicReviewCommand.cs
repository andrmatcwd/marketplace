using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Commands;

public sealed record SubmitPublicReviewCommand(
    int ListingId,
    string UserId,
    string AuthorName,
    string Text,
    int Rating) : IRequest;

internal sealed class SubmitPublicReviewHandler(ICatalogDataService data)
    : IRequestHandler<SubmitPublicReviewCommand>
{
    public Task Handle(SubmitPublicReviewCommand request, CancellationToken cancellationToken)
        => data.SubmitPublicReviewAsync(
            request.ListingId,
            request.UserId,
            request.AuthorName,
            request.Text,
            request.Rating,
            cancellationToken);
}
