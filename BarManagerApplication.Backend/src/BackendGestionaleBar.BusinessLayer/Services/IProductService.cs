using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;

namespace BackendGestionaleBar.BusinessLayer.Services;
public interface IProductService
{
    Task<Product> SaveAsync(SaveProductRequest request);
}
