using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;
using System;
using System.Data;
using System.Threading.Tasks;
using ApplicationProduct = BackendGestionaleBar.Shared.Models.Product;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public interface IProductService
    {
        Task<bool> DeleteProductAsync(Guid id);
        Task<DataTable> GetMenuAsync();
        Task<ApplicationProduct> GetProductAsync(Guid id);
        Task<Response> RegisterProductAsync(RegisterProductRequest request);
    }
}
