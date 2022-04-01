using BackendGestionaleBar.DataAccessLayer.Entities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer
{
    public interface IDataContext : IDisposable
    {
        SqlConnection Connection { get; set; }

        Task<bool> DeleteProductAsync(Guid id);
        Task<Category> GetCategoryAsync(Guid id);
        Task<DataTable> GetMenuAsync();
        Task<Product> GetProductAsync(Guid id);
        Task<bool> RegisterProductAsync(Product product);
    }
}