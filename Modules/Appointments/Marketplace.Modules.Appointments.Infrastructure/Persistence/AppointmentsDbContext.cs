using Marketplace.Modules.Appointments.Domain.Entities;
using Marketplace.Modules.Appointments.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Appointments.Infrastructure.Persistence;

public class AppointmentsDbContext : DbContext
{
    public AppointmentsDbContext(DbContextOptions<AppointmentsDbContext> options) : base(options) { }

    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
    }
}
