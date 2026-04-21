using System;
using Marketplace.Web.Domain.Entities;

namespace Marketplace.Web.Models.Catalog;

public class SubCategoryLookupItem
{
    public SubCategory Entity { get; init; } = default!;
    public int ListingsCount { get; init; }
}
