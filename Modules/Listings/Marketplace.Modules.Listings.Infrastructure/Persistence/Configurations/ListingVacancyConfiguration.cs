using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Configurations;

public sealed class ListingVacancyConfiguration : IEntityTypeConfiguration<ListingVacancy>
{
    public void Configure(EntityTypeBuilder<ListingVacancy> builder)
    {
        builder.ToTable("ListingVacancies");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.EmploymentType).HasMaxLength(100);
        builder.Property(x => x.SalaryText).HasMaxLength(200);
        builder.Property(x => x.LocationText).HasMaxLength(200);
    }
}
