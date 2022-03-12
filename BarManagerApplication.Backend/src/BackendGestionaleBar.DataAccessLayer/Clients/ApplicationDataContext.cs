using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public class ApplicationDataContext : DbContext, IApplicationDataContext
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options)
            : base(options)
        {
        }

        public void Delete<T>(T entity) where T : class
        {
            Set<T>().Remove(entity);
        }

        public void Delete<T>(IEnumerable<T> entities) where T : class
        {
            Set<T>().RemoveRange(entities);
        }

        public Task ExecuteTransactionAsync(Func<Task> action)
        {
            var strategy = Database.CreateExecutionStrategy();
            return strategy.ExecuteAsync(async () =>
            {
                using var transation = await Database.BeginTransactionAsync().ConfigureAwait(false);
                await action.Invoke().ConfigureAwait(false);
            });
        }

        public ValueTask<T> GetAsync<T>(params object[] keyValues) where T : class
            => Set<T>().FindAsync(keyValues);

        public IQueryable<T> GetData<T>(bool trackingChanges = false, bool ignoreQueryFilters = false) where T : class
        {
            var set = Set<T>().AsQueryable();

            if (ignoreQueryFilters)
            {
                set = set.IgnoreQueryFilters();
            }

            return trackingChanges ? set.AsTracking() : set.AsNoTrackingWithIdentityResolution();
        }

        public void Insert<T>(T entity) where T : class
        {
            Set<T>().Add(entity);
        }

        public Task SaveAsync()
            => SaveChangesAsync().ConfigureAwait(false);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Type type = typeof(ApplicationDataContext);
            modelBuilder.ApplyConfigurationsFromAssembly(type.Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}