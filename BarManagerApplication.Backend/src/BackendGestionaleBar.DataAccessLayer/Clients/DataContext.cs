using BackendGestionaleBar.DataAccessLayer.Entities;
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

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

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