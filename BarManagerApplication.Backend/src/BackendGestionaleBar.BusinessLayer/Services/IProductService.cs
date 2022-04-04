using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public interface IProductService
    {
        Task DeleteProductAsync(Guid id);
        Task DeleteProductsAsync();
        Task<List<Menu>> GetMenuAsync();
        Task<Product> GetProductAsync(Guid id);
        Task<Product> SaveProductAsync(SaveProductRequest request);
    }
}