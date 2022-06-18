using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BackendGestionaleBar.DataAccessLayer;

public class DataContext : DbContext, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public void Delete<T>(T entity) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        Set<T>().Remove(entity);
    }
    public void Delete<T>(IEnumerable<T> entities) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));
        Set<T>().RemoveRange(entities);
    }
    public void Edit<T>(T entity) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        Set<T>().Update(entity);
    }
    public void Edit<T>(IEnumerable<T> entities) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));
        Set<T>().UpdateRange(entities);
    }
    public ValueTask<T> GetAsync<T>(params object[] keyValues) where T : BaseEntity
    {
        return Set<T>().FindAsync(keyValues);
    }
    public IQueryable<T> GetData<T>(bool trackingChanges = false, bool ignoreQueryFilters = false) where T : BaseEntity
    {
        var set = Set<T>().AsQueryable<T>();

        if (ignoreQueryFilters)
        {
            set = set.IgnoreQueryFilters();
        }

        return trackingChanges ?
            set.AsTracking() :
            set.AsNoTrackingWithIdentityResolution();
    }
    public void Insert<T>(T entity) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        Set<T>().Add(entity);
    }
    public void Insert<T>(IEnumerable<T> entities) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));
        Set<T>().AddRange(entities);
    }
    public Task<int> SaveAsync() => SaveChangesAsync();
    public Task ExecuteTransactionAsync(Func<Task> action)
    {
        var strategy = Database.CreateExecutionStrategy();

        Task task = strategy.ExecuteAsync(async () =>
        {
            using var transaction = await Database.BeginTransactionAsync().ConfigureAwait(false);
            await action.Invoke().ConfigureAwait(false);
            await transaction.CommitAsync().ConfigureAwait(false);
        });
        return task;
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity.GetType().IsSubclassOf(typeof(BaseEntity))).ToList();

        foreach (var entry in entries.Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            BaseEntity entity = (BaseEntity)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.CreatedDate = DateTime.UtcNow;
                entity.LastModifiedDate = null;
            }
            if (entry.State == EntityState.Modified)
            {
                entity.LastModifiedDate = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}