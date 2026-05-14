using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Queries.GetListingImages;

public sealed record GetListingImagesQuery(int ListingId) : IRequest<IReadOnlyList<ListingImageAdminDto>>;

internal sealed class GetListingImagesHandler(IImageService service)
    : IRequestHandler<GetListingImagesQuery, IReadOnlyList<ListingImageAdminDto>>
{
    public async Task<IReadOnlyList<ListingImageAdminDto>> Handle(GetListingImagesQuery request, CancellationToken cancellationToken)
    {
        var images = await service.GetByListingIdAsync(request.ListingId, cancellationToken);
        return images.Select(i => new ListingImageAdminDto
        {
            Id = i.Id,
            Url = i.Url,
            Alt = i.Alt,
            IsPrimary = i.IsPrimary,
            SortOrder = i.SortOrder
        }).ToList();
    }
}
