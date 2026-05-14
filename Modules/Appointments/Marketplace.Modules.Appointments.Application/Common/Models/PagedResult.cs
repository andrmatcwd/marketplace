namespace Marketplace.Modules.Appointments.Application.Common.Models;

public sealed class PagedResult<T>
{
    public IReadOnlyCollection<T> Items      { get; init; } = Array.Empty<T>();
    public int                    Page       { get; init; }
    public int                    PageSize   { get; init; }
    public int                    TotalCount { get; init; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
}
