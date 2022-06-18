using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;

namespace BackendGestionaleBar.BusinessLayer.Services;
public interface IProductService
{
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Product>> GetAsync(string name);
    Task<Product> SaveAsync(SaveProductRequest request);
}
