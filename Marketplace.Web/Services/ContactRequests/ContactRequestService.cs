using Marketplace.Web.Data;
using Marketplace.Web.Models.Api;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Services.ContactRequests;

public sealed class ContactRequestService : IContactRequestService
{
    private readonly ApplicationDbContext _dbContext;

    public ContactRequestService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ContactRequestResult> CreateAsync(ListingContactRequestDto request, CancellationToken cancellationToken)
    {
        var listingExists = await _dbContext.Listings
            .AsNoTracking()
            .AnyAsync(x => x.IsPublished && x.Id == request.ListingId, cancellationToken);

        if (!listingExists)
        {
            return new ContactRequestResult
            {
                Success = false,
                Message = "Listing not found."
            };
        }

        // Тут пізніше можна:
        // - зберігати в БД
        // - відправляти email власнику listing
        // - слати в Telegram / CRM
        // - логувати подію

        return new ContactRequestResult
        {
            Success = true
        };
    }
}