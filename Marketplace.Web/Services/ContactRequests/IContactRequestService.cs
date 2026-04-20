using Marketplace.Web.Models.Api;

namespace Marketplace.Web.Services.ContactRequests;

public interface IContactRequestService
{
    Task<ContactRequestResult> CreateAsync(ListingContactRequestDto request, CancellationToken cancellationToken);
}

public sealed class ContactRequestResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}