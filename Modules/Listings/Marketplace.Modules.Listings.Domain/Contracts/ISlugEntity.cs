using System;

namespace Marketplace.Modules.Listings.Domain.Contracts;

public interface ISlugEntity
{
    public string Name { get; set; }
    public string Slug { get; set; }
}
