using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Type type = typeof(ApplicationDbContext);
            modelBuilder.ApplyConfigurationsFromAssembly(type.Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}