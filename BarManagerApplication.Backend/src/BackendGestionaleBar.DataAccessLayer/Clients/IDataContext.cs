using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public interface IDataContext
    {
        DbSet<Allergen> Allergens { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductAllergen> ProductAllergens { get; set; }

        Task<bool> SaveAsync();
    }
}