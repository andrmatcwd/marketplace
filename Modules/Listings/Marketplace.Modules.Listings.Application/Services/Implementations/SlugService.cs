using System;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class SlugService : ISlugService
{
    public string Generate(string value)
    {
        return value
            .Trim()
            .ToLowerInvariant()
            .Replace(" ", "-");
    }
}
