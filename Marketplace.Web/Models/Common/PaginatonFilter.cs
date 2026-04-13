using System;

namespace Marketplace.Web.Models.Common;

public class PaginatonFilter : BaseFilter
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
}
