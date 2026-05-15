using Marketplace.Modules.Listings.Application.Catalog.Commands;
using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Navigation;
using MediatR;

namespace Marketplace.Web.Services.Listings;

public sealed class ListingService : IListingService
{
    private readonly IMediator _mediator;
    private readonly IListingVmMapper _mapper;
    private readonly ICatalogBreadcrumbBuilder _breadcrumbBuilder;

    public ListingService(
        IMediator mediator,
        IListingVmMapper mapper,
        ICatalogBreadcrumbBuilder breadcrumbBuilder)
    {
        _mediator = mediator;
        _mapper = mapper;
        _breadcrumbBuilder = breadcrumbBuilder;
    }

    public async Task<ListingDetailsPageVm?> GetDetailsPageAsync(
        string culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        string serviceSlug,
        int id,
        CancellationToken cancellationToken)
    {
        var dto = await _mediator.Send(new GetListingDetailsQuery(id), cancellationToken);
        if (dto is null) return null;

        var related = await _mediator.Send(
            new GetRelatedListingsQuery(dto.Id, dto.CityId, dto.SubCategoryId), cancellationToken);

        var relatedListings = related.Select(x => _mapper.MapRelatedListing(x, culture)).ToList();

        var vm = _mapper.MapDetails(dto, culture, relatedListings);

        vm.Breadcrumbs = _breadcrumbBuilder.BuildListing(
            culture,
            dto.Title,
            dto.CityName,
            dto.CitySlug,
            dto.CategoryName,
            dto.CategorySlug,
            dto.SubCategoryName,
            dto.SubCategorySlug);

        return vm;
    }

    public Task SubmitReviewAsync(
        int listingId, string userId, string authorName, string text, int rating,
        CancellationToken cancellationToken)
        => _mediator.Send(
            new SubmitPublicReviewCommand(listingId, userId, authorName, text, rating),
            cancellationToken);
}
