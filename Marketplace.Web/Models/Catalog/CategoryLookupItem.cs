using System;
using Marketplace.Web.Domain.Entities;

namespace Marketplace.Web.Models.Catalog;

public class CategoryLookupItem
{
    public Category Entity { get; init; } = default!;

    public int ListingsCount { get; init; }
}
