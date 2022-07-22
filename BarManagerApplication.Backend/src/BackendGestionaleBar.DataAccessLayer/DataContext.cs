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
using TinyHelpers.Extensions;

namespace BackendGestionaleBar.DataAccessLayer;

public sealed class DataContext : DbContext, IDataContext
{
    private static readonly MethodInfo _setQueryFilter;
    private static readonly Type _dataContextType;

    private readonly IUserService _userService;
    private readonly ILogger<DataContext> _logger;

    private SqlConnection _connection;
    private SqlCommand _command;
    private SqlDataReader _dataReader;

    private bool _disposed;

    //constructors
    static DataContext()
    {
        _dataContextType = typeof(DataContext);
        _setQueryFilter = _dataContextType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == nameof(SetQueryFilter));
    }
    public DataContext(DbContextOptions<DataContext> options, IUserService userService, ILogger<DataContext> logger) : base(options)
    {
        _userService = userService;
        _logger = logger;

        _connection = null;
        _command = null;
        _dataReader = null;

        _disposed = false;

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

            SqlConnection connection = _connection;
            Exception e = null;

            //attempting close the active connection before returning it
            if (connection.State is ConnectionState.Open)
            {
                try
                {
                    _logger.LogInformation("closing connection");
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
                    _logger.LogError(e, "Error");
                    throw e;
                }
            }

            return connection;
        }
        set
        {
            ThrowIfDisposed();

            Exception e = null;

            if (value != null)
            {
                e = new Exception("please provide a valid connection");
                _logger.LogError(e, "Error");
                throw e;
            }

            SqlConnection connection = (SqlConnection)value;

            try
            {
                _logger.LogInformation("testing connection");
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
                _logger.LogError(e, "Error");
                throw e;
            }

            _connection = connection;
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

        _dataReader = await GetTableAsync("Menu").ConfigureAwait(false);
        List<Menu> result;

        if (_dataReader == null)
        {
            result = null;
        }
        else
        {
            result = new List<Menu>();

            while (_dataReader.Read())
            {
                Menu menu = new()
                {
                    Product = Convert.ToString(_dataReader["Product"]),
                    Category = Convert.ToString(_dataReader["Category"]),
                    Price = Convert.ToDecimal(_dataReader["Price"]),
                    Quantity = Convert.ToInt32(_dataReader["Quantity"])
                };

                result.Add(menu);
            }
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

    //helper methods
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity.GetType().IsSubclassOf(typeof(BaseEntity))).ToList();

        Guid? userId = _userService.GetId();

        foreach (var entry in entries.Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
        {
            BaseEntity baseEntity = (BaseEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                _logger.LogInformation("Saving entity");

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
                _logger.LogInformation("Updating entity");

                baseEntity.LastModifiedDate = DateTime.UtcNow;
                baseEntity.UpdatedBy = userId.GetValueOrDefault(Guid.Empty);
            }
            if (entry.State == EntityState.Deleted)
            {
                _logger.LogInformation("Deleting entity");

                if (baseEntity is DeletableEntity deletableEntity)
                {
                    entry.State = EntityState.Modified;
                    deletableEntity.IsDeleted = true;
                    deletableEntity.DeletedDate = DateTime.UtcNow;
                    deletableEntity.DeletedBy = userId.GetValueOrDefault(Guid.Empty);
                }
            }
        }

        _logger.LogInformation("Applying changes to the database");
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
        if (disposing && !_disposed)
        {
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                _connection.Dispose();
            }

            if (_command != null)
            {
                _command.Dispose();
            }

            if (_dataReader != null)
            {
                _dataReader.Dispose();
            }

            _disposed = true;
        }
    }
    private void ThrowIfDisposed()
    {
        bool disposed = _disposed;
        string name = _dataContextType.Name;

        if (disposed)
        {
            throw new ObjectDisposedException(name);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //applying configurations
        Assembly assembly = Assembly.GetExecutingAssembly();
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

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
    private async Task<SqlDataReader> GetTableAsync(string tableName)
    {
        SqlDataReader reader;
        Exception e = null;

        try
        {
            await _connection.OpenAsync().ConfigureAwait(false);
            _command = _connection.CreateCommand();
            _command.CommandText = $"SELECT * FROM {tableName}";
            reader = await _command.ExecuteReaderAsync().ConfigureAwait(false);
            await _connection.CloseAsync().ConfigureAwait(false);
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
            _logger.LogError(e, "Error");
        }

        return reader;
    }
    private void Configure()
    {
        Exception e = null;
        string connectionString = Database.GetConnectionString();
        bool hasValue = connectionString.HasValue();

        if (!hasValue)
        {
            e = new Exception("please provide a valid connection string");
            _logger.LogError(e, "Error");
            throw e;
        }

        SqlConnection connection = new(connectionString);

        try
        {
            _logger.LogInformation("Testing connection");
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
            _logger.LogError(e, "Error");
            throw e;
        }
        else
        {
            _logger.LogInformation("Test connection succedeed");
            _connection = connection;
        }
    }
    private void SetQueryFilter<T>(ModelBuilder builder) where T : DeletableEntity
    {
        builder.Entity<T>().HasQueryFilter(x => !x.IsDeleted && x.DeletedDate == null && x.DeletedBy == null);
    }
    private static IEnumerable<MethodInfo> SetGlobalQueryMethods(Type type)
    {
        List<MethodInfo> result = new();
        Type deletableEntityType = typeof(DeletableEntity);

        if (deletableEntityType.IsAssignableFrom(type))
        {
            result.Add(_setQueryFilter);
        }

        return result;
    }
}