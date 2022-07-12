using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using BackendGestionaleBar.DataAccessLayer.Views;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;

namespace BackendGestionaleBar.DataAccessLayer;

public sealed class DataContext : DbContext, IDataContext
{
    private static readonly MethodInfo setQueryFilter;

    private readonly ILogger<DataContext> logger;

    private List<EntityEntry> entries = null;
    private SqlConnection sqlConnection = null;

    static DataContext()
    {
        setQueryFilter = typeof(DataContext).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == nameof(SetQueryFilter));
    }
    public DataContext(DbContextOptions<DataContext> options, ILogger<DataContext> logger) : base(options)
    {
        this.logger = logger;
        Configure();
    }

    public void Delete<T>(T entity) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        var set = Set<T>();
        set.Remove(entity);
    }
    public void Delete<T>(IEnumerable<T> entities) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));
        var set = Set<T>();
        set.RemoveRange(entities);
    }
    public void Edit<T>(T entity) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        var set = Set<T>();
        set.Update(entity);
    }
    public ValueTask<T> GetAsync<T>(params object[] keyValues) where T : BaseEntity
    {
        var set = Set<T>();
        return set.FindAsync(keyValues);
    }
    public async Task<List<Menu>> GetMenuAsync()
    {
        using var reader = await ExecuteReaderAsync("SELECT * FROM Menu").ConfigureAwait(false);

        if (reader == null)
        {
            return null;
        }
        else
        {
            var result = new List<Menu>();

            while (reader.Read())
            {
                var menu = new Menu
                {
                    Product = Convert.ToString(reader["Product"]),
                    Category = Convert.ToString(reader["Category"]),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Quantity = Convert.ToInt32(reader["Quantity"])
                };

                result.Add(menu);
            }

            return result;
        }
    }
    public IQueryable<T> GetData<T>(bool trackingChanges = false, bool ignoreQueryFilters = false) where T : BaseEntity
    {
        var set = Set<T>().AsQueryable();

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
        var set = Set<T>();
        set.Add(entity);
    }
    public Task SaveAsync() => SaveChangesAsync();
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
    public override void Dispose()
    {
        sqlConnection.Dispose();
        base.Dispose();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        entries = ChangeTracker.Entries()
            .Where(e => e.Entity.GetType().IsSubclassOf(typeof(BaseEntity))).ToList();

        try
        {
            foreach (var entry in entries.Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
            {
                BaseEntity baseEntity = (BaseEntity)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    logger.LogInformation("Saving entity . . .");
                    baseEntity.CreatedDate = DateTime.UtcNow;
                    baseEntity.LastModifiedDate = null;
                    if (baseEntity is DeletableEntity deletableEntity)
                    {
                        deletableEntity.IsDeleted = false;
                        deletableEntity.DeletedDate = null;
                    }
                }
                if (entry.State == EntityState.Modified)
                {
                    logger.LogInformation("Updating entity . . .");
                    baseEntity.LastModifiedDate = DateTime.UtcNow;
                }
                if (entry.State == EntityState.Deleted)
                {
                    logger.LogInformation("Deleting entity . . .");
                    if (baseEntity is DeletableEntity deletableEntity)
                    {
                        entry.State = EntityState.Modified;
                        deletableEntity.IsDeleted = true;
                        deletableEntity.DeletedDate = DateTime.UtcNow;
                    }
                }
            }

            logger.LogInformation("Applying changes to the database . . .");
            return base.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error");
            return Task.FromResult(0);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var trimStringConverter = new ValueConverter<string, string>(v => v.Trim(), v => v.Trim());
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(string))
                {
                    modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion(trimStringConverter);
                }
            }
        }

        var entities = modelBuilder.Model.GetEntityTypes()
            .Where(t => typeof(DeletableEntity).IsAssignableFrom(t.ClrType)).ToList();

        foreach (var type in entities.Select(t => t.ClrType))
        {
            var methods = SetGlobalQueryMethods(type);

            foreach (var method in methods)
            {
                var genericMethod = method.MakeGenericMethod(type);
                genericMethod.Invoke(this, new object[] { modelBuilder });
            }
        }

        base.OnModelCreating(modelBuilder);
    }
    private async Task<SqlDataReader> ExecuteReaderAsync(string commandText)
    {
        SqlCommand sqlCommand;
        SqlDataReader reader;

        try
        {
            await sqlConnection.OpenAsync().ConfigureAwait(false);
            sqlCommand = new SqlCommand(commandText, sqlConnection);
            reader = await sqlCommand.ExecuteReaderAsync().ConfigureAwait(false);
            await sqlConnection.CloseAsync().ConfigureAwait(false);
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, "Error");
            reader = null;
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Error");
            reader = null;
        }

        return reader;
    }
    private void Configure()
    {
        string connectionString = Database.GetConnectionString();
        sqlConnection = new SqlConnection(connectionString);
        if (sqlConnection.State is ConnectionState.Open)
        {
            sqlConnection.Close();
        }
    }
    private void SetQueryFilter<T>(ModelBuilder modelBuilder) where T : DeletableEntity
    {
        modelBuilder.Entity<T>().HasQueryFilter(x => !x.IsDeleted && x.DeletedDate == null);
    }
    private static IEnumerable<MethodInfo> SetGlobalQueryMethods(Type type)
    {
        var result = new List<MethodInfo>();

        Type deletableEntityType = typeof(DeletableEntity);

        if (deletableEntityType.IsAssignableFrom(type))
        {
            result.Add(setQueryFilter);
        }

        return result;
    }
}