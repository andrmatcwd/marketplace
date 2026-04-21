using System;
using Marketplace.Web.Domain.Entities;

namespace Marketplace.Web.Models.Catalog;

public class CityLookupItem
{
    public City Entity { get; init; } = default!;

    public int ListingsCount { get; init; }
}
