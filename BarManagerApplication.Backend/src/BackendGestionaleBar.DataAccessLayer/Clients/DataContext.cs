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

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            Set<T>().Remove(entity);
            await SaveChangesAsync();
        }

        public async ValueTask<T> GetAsync<T>(params object[] keyValues) where T : class
        {
            return await Set<T>().FindAsync(keyValues);
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            Set<T>().Add(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            Set<T>().Update(entity);
            await SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Type type = typeof(DataContext);
            modelBuilder.ApplyConfigurationsFromAssembly(type.Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}