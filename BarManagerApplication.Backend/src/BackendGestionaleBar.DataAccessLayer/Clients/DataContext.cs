using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public void Create<T>(T entity) where T : BaseEntity
        {
            var set = Set<T>();
            set.Add(entity);
        }
        public void Delete<T>(T entity) where T : BaseEntity
        {
            var set = Set<T>();
            set.Remove(entity);
        }
        public async Task<T> ReadAsync<T>(params object[] keyValues) where T : BaseEntity
        {
            var set = Set<T>();
            var entity = await set.FindAsync(keyValues);
            return entity;
        }
        public IQueryable<T> GetData<T>(bool trackingChanges = false) where T : BaseEntity
        {
            var set = Set<T>();
            return trackingChanges ? set.AsTracking() : set.AsNoTracking();
        }
        public async Task SaveAsync() => await SaveChangesAsync();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Type type = typeof(DataContext);
            modelBuilder.ApplyConfigurationsFromAssembly(type.Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}