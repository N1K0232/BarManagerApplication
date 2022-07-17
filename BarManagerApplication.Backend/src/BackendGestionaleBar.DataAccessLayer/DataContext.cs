using BackendGestionaleBar.Contracts;
using BackendGestionaleBar.DataAccessLayer.Entities;
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

    private readonly IUserService userService;
    private readonly ILogger<DataContext> logger;

    private SqlConnection sqlConnection;
    private SqlCommand sqlCommand;
    private SqlDataReader sqlDataReader;

    static DataContext()
    {
        Type type = typeof(DataContext);
        setQueryFilter = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == nameof(SetQueryFilter));
    }
    public DataContext(DbContextOptions<DataContext> options, IUserService userService, ILogger<DataContext> logger) : base(options)
    {
        this.userService = userService;
        this.logger = logger;

        sqlConnection = null;
        sqlCommand = null;
        sqlDataReader = null;

        Configure();
    }

    public DbSet<OrderDetail> OrderDetails
    {
        get
        {
            return Set<OrderDetail>();
        }
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

    public ValueTask<T> GetAsync<T>(params object[] keyValues) where T : BaseEntity
    {
        return Set<T>().FindAsync(keyValues);
    }

    public async Task<List<Menu>> GetMenuAsync()
    {
        sqlDataReader = await ExecuteReaderAsync("Menu").ConfigureAwait(false);
        List<Menu> result;

        if (sqlDataReader == null)
        {
            result = null;
        }
        else
        {
            result = new List<Menu>();

            while (sqlDataReader.Read())
            {
                Menu menu = new()
                {
                    Product = Convert.ToString(sqlDataReader["Product"]),
                    Category = Convert.ToString(sqlDataReader["Category"]),
                    Price = Convert.ToDecimal(sqlDataReader["Price"]),
                    Quantity = Convert.ToInt32(sqlDataReader["Quantity"])
                };

                result.Add(menu);
            }

            sqlDataReader.Dispose();
        }

        return result;
    }

    public IQueryable<T> GetData<T>(bool trackingChanges = false, bool ignoreQueryFilters = false) where T : BaseEntity
    {
        IQueryable<T> set = Set<T>().AsQueryable();

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
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity.GetType().IsSubclassOf(typeof(BaseEntity))).ToList();

        Guid? userId = userService.GetId();

        foreach (var entry in entries.Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
        {
            BaseEntity baseEntity = (BaseEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                logger.LogInformation("Saving entity");

                baseEntity.CreatedDate = DateTime.UtcNow;
                baseEntity.CreatedBy = userId.GetValueOrDefault(Guid.Empty);
                baseEntity.LastModifiedDate = null;
                baseEntity.UpdatedBy = null;

                if (baseEntity is DeletableEntity deletableEntity)
                {
                    deletableEntity.IsDeleted = false;
                    deletableEntity.DeletedDate = null;
                    deletableEntity.DeletedBy = null;
                }
            }
            if (entry.State == EntityState.Modified)
            {
                logger.LogInformation("Updating entity");

                baseEntity.LastModifiedDate = DateTime.UtcNow;
                baseEntity.UpdatedBy = userId.GetValueOrDefault(Guid.Empty);
            }
            if (entry.State == EntityState.Deleted)
            {
                logger.LogInformation("Deleting entity");

                if (baseEntity is DeletableEntity deletableEntity)
                {
                    entry.State = EntityState.Modified;
                    deletableEntity.IsDeleted = true;
                    deletableEntity.DeletedDate = DateTime.UtcNow;
                    deletableEntity.DeletedBy = userId.GetValueOrDefault(Guid.Empty);
                }
            }
        }

        logger.LogInformation("Applying changes to the database");
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //applying converter for strings. When the runtime gets, adds or updates an entity
        //the runtime trims the strings
        var trimStringConverter = new ValueConverter<string, string>(v => v.Trim(), v => v.Trim());
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(string))
                {
                    modelBuilder.Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(trimStringConverter);
                }
            }
        }

        //applying query filter on DeletableEntity objects
        var entities = modelBuilder.Model
            .GetEntityTypes()
            .Where(t => typeof(DeletableEntity).IsAssignableFrom(t.ClrType))
            .ToList();
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

    private async Task<SqlDataReader> ExecuteReaderAsync(string tableName)
    {
        SqlDataReader reader;
        Exception e = null;

        try
        {
            await sqlConnection.OpenAsync().ConfigureAwait(false);
            sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"SELECT * FROM {tableName}";
            reader = await sqlCommand.ExecuteReaderAsync().ConfigureAwait(false);
            await sqlConnection.CloseAsync().ConfigureAwait(false);
            sqlCommand.Dispose();
        }
        catch (SqlException ex)
        {
            e = ex;
            reader = null;
        }
        catch (InvalidOperationException ex)
        {
            e = ex;
            reader = null;
        }

        if (e != null)
        {
            logger.LogError(e, "Error");
        }

        return reader;
    }

    private void Configure()
    {
        string connectionString = Database.GetConnectionString();
        SqlConnection connection = new(connectionString);
        Exception e = null;

        try
        {
            logger.LogInformation("Testing connection");
            connection.Open();
            connection.Close();
        }
        catch (SqlException ex)
        {
            e = ex;
        }
        catch (InvalidOperationException ex)
        {
            e = ex;
        }

        if (e != null)
        {
            logger.LogError(e, "Error");
            throw e;
        }
        else
        {
            logger.LogInformation("Test connection succedeed");
            sqlConnection = connection;
        }
    }

    private void SetQueryFilter<T>(ModelBuilder modelBuilder) where T : DeletableEntity
    {
        modelBuilder.Entity<T>().HasQueryFilter(x => !x.IsDeleted && x.DeletedDate == null && x.DeletedBy == null);
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