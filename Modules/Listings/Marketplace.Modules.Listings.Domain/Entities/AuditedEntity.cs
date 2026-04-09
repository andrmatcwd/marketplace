namespace Marketplace.Modules.Listings.Domain.Entities;

public class AuditedEntity
{
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
