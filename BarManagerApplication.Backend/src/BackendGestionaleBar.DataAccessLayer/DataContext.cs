using BackendGestionaleBar.Contracts;
using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using BackendGestionaleBar.DataAccessLayer.Views;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;

namespace BackendGestionaleBar.DataAccessLayer;

public sealed class DataContext : DbContext, IDataContext, ISqlContext
{
    private static readonly MethodInfo setQueryFilter;
    private static readonly Type dataContextType;

    private readonly IUserService userService;
    private readonly ILogger<DataContext> logger;

    private SqlConnection activeConnection;
    private SqlCommand command;
    private SqlDataReader dataReader;

    private bool disposed;

    //constructors
    static DataContext()
    {
        dataContextType = typeof(DataContext);
        setQueryFilter = dataContextType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == nameof(SetQueryFilter));
    }
    public DataContext(DbContextOptions<DataContext> options, IUserService userService, ILogger<DataContext> logger) : base(options)
    {
        this.userService = userService;
        this.logger = logger;

        activeConnection = null;
        command = null;
        dataReader = null;

        disposed = false;

        Configure();
    }

    //destructor
    ~DataContext()
    {
        Dispose(false);
    }

    //properties
    public DbSet<OrderDetail> OrderDetails
    {
        get
        {
            ThrowIfDisposed();
            return Set<OrderDetail>();
        }
    }
    public IDbConnection Connection
    {
        get
        {
            ThrowIfDisposed();

            if (activeConnection.State is ConnectionState.Open)
            {
                //attempting close the active connection before returning it
                Exception e = null;

                try
                {
                    logger.LogInformation("closing connection");
                    activeConnection.Close();
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
            }

            return activeConnection;
        }
    }
    private IDbConnection InternalConnection
    {
        get
        {
            ThrowIfDisposed();

            if (activeConnection.State is ConnectionState.Closed)
            {
                Exception e = null;

                try
                {
                    logger.LogInformation("opening connection");
                    activeConnection.Open();
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
            }

            return activeConnection;
        }
    }

    //IDataContext interface implemented methods
    public void Delete<T>(T entity) where T : BaseEntity
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        Set<T>().Remove(entity);
    }
    public void Delete<T>(IEnumerable<T> entities) where T : BaseEntity
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));
        Set<T>().RemoveRange(entities);
    }
    public void Edit<T>(T entity) where T : BaseEntity
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        Set<T>().Update(entity);
    }
    public ValueTask<T> GetAsync<T>(params object[] keyValues) where T : BaseEntity
    {
        ThrowIfDisposed();
        return Set<T>().FindAsync(keyValues);
    }
    public async Task<List<Menu>> GetMenuAsync()
    {
        ThrowIfDisposed();

        dataReader = await ExecuteReaderAsync("Menu").ConfigureAwait(false);
        List<Menu> result;

        if (dataReader == null)
        {
            result = null;
        }
        else
        {
            result = new List<Menu>();

            while (dataReader.Read())
            {
                Menu menu = new()
                {
                    Product = Convert.ToString(dataReader["Product"]),
                    Category = Convert.ToString(dataReader["Category"]),
                    Price = Convert.ToDecimal(dataReader["Price"]),
                    Quantity = Convert.ToInt32(dataReader["Quantity"])
                };

                result.Add(menu);
            }

            dataReader.Dispose();
        }

        return result;
    }
    public IQueryable<T> GetData<T>(bool trackingChanges = false, bool ignoreQueryFilters = false) where T : BaseEntity
    {
        ThrowIfDisposed();

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
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        Set<T>().Add(entity);
    }
    public Task SaveAsync() => SaveChangesAsync();
    public Task ExecuteTransactionAsync(Func<Task> action)
    {
        ThrowIfDisposed();

        var strategy = Database.CreateExecutionStrategy();

        Task task = strategy.ExecuteAsync(async () =>
        {
            using var transaction = await Database.BeginTransactionAsync().ConfigureAwait(false);
            await action.Invoke().ConfigureAwait(false);
            await transaction.CommitAsync().ConfigureAwait(false);
        });

        return task;
    }

    //ISqlContext interface implemented methods
    public Task<IEnumerable<T>> GetDataAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        where T : class
    {
        ThrowIfDisposed();
        return InternalConnection.QueryAsync<T>(sql, param, transaction, commandType: commandType);
    }
    public Task<IEnumerable<TReturn>> GetDataAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TReturn : class
    {
        ThrowIfDisposed();
        return InternalConnection.QueryAsync(sql, map, param, transaction, splitOn: splitOn, commandType: commandType);
    }
    public Task<IEnumerable<TReturn>> GetDataAsync<TFirst, TSecond, TThrid, TReturn>(string sql, Func<TFirst, TSecond, TThrid, TReturn> map, object param = null, IDbTransaction transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TThrid : class
        where TReturn : class
    {
        ThrowIfDisposed();
        return InternalConnection.QueryAsync(sql, map, param, transaction, splitOn: splitOn, commandType: commandType);
    }
    public Task<IEnumerable<TReturn>> GetDataAsync<TFirst, TSecond, TThrid, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThrid, TFourth, TReturn> map, object param = null, IDbTransaction transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TThrid : class
        where TFourth : class
        where TReturn : class
    {
        ThrowIfDisposed();
        return InternalConnection.QueryAsync(sql, map, param, transaction, splitOn: splitOn, commandType: commandType);
    }
    public Task<T> GetObjectAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        where T : class
    {
        ThrowIfDisposed();
        return InternalConnection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandType: commandType);
    }
    public async Task<TReturn> GetObjectAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TReturn : class
    {
        ThrowIfDisposed();

        var result = await InternalConnection.QueryAsync(sql, map, param, transaction, splitOn: splitOn, commandType: commandType).ConfigureAwait(false);
        return result.FirstOrDefault();
    }
    public async Task<TReturn> GetObjectAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TThird : class
        where TReturn : class
    {
        ThrowIfDisposed();

        var result = await InternalConnection.QueryAsync(sql, map, param, transaction, splitOn: splitOn, commandType: commandType).ConfigureAwait(false);
        return result.FirstOrDefault();
    }
    public async Task<TReturn> GetObjectAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, IDbTransaction transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TThird : class
        where TFourth : class
        where TReturn : class
    {
        ThrowIfDisposed();

        var result = await InternalConnection.QueryAsync(sql, map, param, transaction, splitOn: splitOn, commandType: commandType).ConfigureAwait(false);
        return result.FirstOrDefault();
    }
    public Task<T> GetSingleValueAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
    {
        ThrowIfDisposed();
        return InternalConnection.ExecuteScalarAsync<T>(sql, param, transaction, commandType: commandType);
    }
    public Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
    {
        ThrowIfDisposed();
        return InternalConnection.ExecuteAsync(sql, param, transaction, commandType: commandType);
    }
    public IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
    {
        ThrowIfDisposed();
        return InternalConnection.BeginTransaction(isolationLevel);
    }

    //helper methods
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

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
    public override void Dispose()
    {
        Dispose(true);
        base.Dispose();
        GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            if (activeConnection != null)
            {
                if (activeConnection.State == ConnectionState.Open)
                {
                    activeConnection.Close();
                }
                activeConnection.Dispose();
            }

            if (command != null)
            {
                command.Dispose();
            }

            if (dataReader != null)
            {
                dataReader.Dispose();
            }

            disposed = true;
        }
    }
    private void ThrowIfDisposed()
    {
        if (disposed)
        {
            throw new ObjectDisposedException(dataContextType.Name);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //applying configurations
        modelBuilder.ApplyConfigurationsFromAssembly(dataContextType.Assembly);

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
            await activeConnection.OpenAsync().ConfigureAwait(false);
            command = activeConnection.CreateCommand();
            command.CommandText = $"SELECT * FROM {tableName}";
            reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
            await activeConnection.CloseAsync().ConfigureAwait(false);
            command.Dispose();
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
            activeConnection = connection;
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