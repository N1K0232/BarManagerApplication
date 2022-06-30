using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

namespace BackendGestionaleBar.DataAccessLayer;

public class DataContext : AuthenticationDataContext, IDataContext
{
    public DataContext(DbContextOptions<AuthenticationDataContext> options) : base(options)
    {
    }

    public void Delete<T>(T entity) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        DbSet<T> set = Set<T>();
        set.Remove(entity);
    }
    public void Delete<T>(IEnumerable<T> entities) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));

        DbSet<T> set = Set<T>();
        set.RemoveRange(entities);
    }
    public void Edit<T>(T entity) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        DbSet<T> set = Set<T>();
        set.Update(entity);
    }
    public void Edit<T>(IEnumerable<T> entities) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));

        DbSet<T> set = Set<T>();
        set.UpdateRange(entities);
    }
    public ValueTask<T> GetAsync<T>(params object[] keyValues) where T : BaseEntity
    {
        DbSet<T> set = Set<T>();
        return set.FindAsync(keyValues);
    }
    public IQueryable<T> GetData<T>(bool trackingChanges = false, bool ignoreQueryFilters = false) where T : BaseEntity
    {
        IQueryable<T> set = Set<T>().AsQueryable<T>();

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

        DbSet<T> set = Set<T>();
        set.Add(entity);
    }
    public void Insert<T>(IEnumerable<T> entities) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));

        DbSet<T> set = Set<T>();
        set.AddRange(entities);
    }
    public Task SaveAsync() => SaveChangesAsync();
    public Task ExecuteTransactionAsync(Func<Task> action)
    {
        IExecutionStrategy strategy = Database.CreateExecutionStrategy();

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