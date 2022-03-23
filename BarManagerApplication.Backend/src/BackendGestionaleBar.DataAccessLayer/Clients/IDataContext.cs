using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public interface IDataContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Product> Products { get; set; }

        Task<bool> SaveAsync();
    }
}