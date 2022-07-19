using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using BackendGestionaleBar.DataAccessLayer.Views;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BackendGestionaleBar.DataAccessLayer;

public interface IDataContext
{
    DbSet<OrderDetail> OrderDetails { get; }
    IDbConnection Connection { get; }

    void Delete<T>(T entity) where T : BaseEntity;

    void Delete<T>(IEnumerable<T> entities) where T : BaseEntity;

    void Edit<T>(T entity) where T : BaseEntity;

    IQueryable<T> GetData<T>(bool trackingChanges = false, bool ignoreQueryFilters = false) where T : BaseEntity;

    ValueTask<T> GetAsync<T>(params object[] keyValues) where T : BaseEntity;

    Task<List<Menu>> GetMenuAsync();

    void Insert<T>(T entity) where T : BaseEntity;

    Task SaveAsync();

    Task ExecuteTransactionAsync(Func<Task> action);
}