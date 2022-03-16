using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public void Delete<T>(T entity) where T : class
        {
            Set<T>().Remove(entity);
        }
        public async ValueTask<T> GetAsync<T>(params object[] keyValues) where T : class
        {
            return await Set<T>().FindAsync(keyValues);
        }
        public void Insert<T>(T entity) where T : class
        {
            Set<T>().Add(entity);
        }
        public void Edit<T>(T entity) where T : class
        {
            Set<T>().Update(entity);
        }

        public async Task<bool> SaveAsync()
        {
            bool result;
            try
            {
                await SaveChangesAsync();
                result = true;
            }
            catch (DbUpdateConcurrencyException)
            {
                result = false;
            }
            catch (DbUpdateException)
            {
                result = false;
            }
            return result;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Type type = typeof(DataContext);
            modelBuilder.ApplyConfigurationsFromAssembly(type.Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}