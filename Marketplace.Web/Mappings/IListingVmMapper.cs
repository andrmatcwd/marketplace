using Marketplace.Web.Domain.Entities;
using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Mappings;

public interface IListingVmMapper
{
    ListingDetailsPageVm MapDetails(Listing entity, string culture, IReadOnlyCollection<RelatedListingVm>? relatedListings = null);
    ListingReviewVm MapReview(ListingReview entity);
    RelatedListingVm MapRelatedListing(Listing entity, string culture);
}