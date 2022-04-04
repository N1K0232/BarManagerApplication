using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;
using System.Data;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public interface IProductService
    {
        Task DeleteProductAsync(Guid id);
        Task DeleteProductsAsync();
        Task<DataTable> GetMenuAsync();
        Task<Product> GetProductAsync(Guid id);
        Task<Product> SaveProductAsync(SaveProductRequest request);
    }
}