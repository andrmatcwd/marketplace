namespace Marketplace.Web.Services.AiSearch;

public interface IAiSearchService
{
    Task<AiSearchResult> SearchAsync(string description, string culture, CancellationToken cancellationToken = default);
}

public sealed record AiSearchResult(bool Success, string? RedirectUrl, string? ErrorMessage);
