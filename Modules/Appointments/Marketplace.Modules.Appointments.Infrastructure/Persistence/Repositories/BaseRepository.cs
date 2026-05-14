using Marketplace.Modules.Appointments.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Appointments.Infrastructure.Persistence.Repositories;

internal abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    where TEntity : class
{
    protected readonly AppointmentsDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    protected BaseRepository(AppointmentsDbContext context)
    {
        Context = context;
        DbSet   = context.Set<TEntity>();
    }

    public virtual Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        => DbSet.FindAsync([id], cancellationToken).AsTask();

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => await DbSet.ToListAsync(cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await DbSet.AddAsync(entity, cancellationToken);

    public void Update(TEntity entity) => DbSet.Update(entity);

    public void Remove(TEntity entity) => DbSet.Remove(entity);

    public Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
        => DbSet.FindAsync([id], cancellationToken).AsTask().ContinueWith(t => t.Result is not null);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => Context.SaveChangesAsync(cancellationToken);
}
