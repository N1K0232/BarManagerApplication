using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BackendGestionaleBar.DataAccessLayer
{
    public class DataContext : DbContext, IDataContext, IReadOnlyDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public void Delete<T>(T entity) where T : BaseEntity
        {
            var set = Set<T>();
            set.Remove(entity);
        }
        public void Delete<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            var set = Set<T>();
            set.RemoveRange(entities);
        }
        public async Task<T> GetAsync<T>(params object[] keyValues) where T : BaseEntity
        {
            var set = Set<T>();
            var entity = await set.FindAsync(keyValues);
            return entity;
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
                set.AsNoTracking();
        }
        public void Insert<T>(T entity) where T : BaseEntity
        {
            entity.CreatedDate = DateTime.UtcNow;
            var set = Set<T>();
            set.Add(entity);
        }
        public async Task SaveAsync() => await SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Type currentType = typeof(DataContext);
            Assembly currentAssembly = currentType.Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(currentAssembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}