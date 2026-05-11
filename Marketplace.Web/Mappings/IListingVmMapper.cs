using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Mappings;

public interface IListingVmMapper
{
    ListingDetailsPageVm MapDetails(ListingDetailsDto dto, string culture, IReadOnlyCollection<RelatedListingVm>? relatedListings = null);
    RelatedListingVm MapRelatedListing(ListingCardDto dto, string culture);
}
