using Marketplace.Modules.Blog.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Blog.Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    where TEntity : class
{
    protected readonly BlogDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    public BaseRepository(BlogDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        => await DbSet.FindAsync([id!], cancellationToken);

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => await DbSet.AsNoTracking().ToListAsync(cancellationToken);

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await DbSet.AddAsync(entity, cancellationToken);

    public virtual void Update(TEntity entity) => DbSet.Update(entity);

    public virtual void Remove(TEntity entity) => DbSet.Remove(entity);

    public virtual async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
        => await GetByIdAsync(id, cancellationToken) is not null;

    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => DbContext.SaveChangesAsync(cancellationToken);
}
