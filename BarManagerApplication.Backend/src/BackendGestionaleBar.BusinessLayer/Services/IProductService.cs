using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public interface IProductService
    {
        Task<bool> DeleteProductAsync(Guid id);
        Task<DataTable> GetMenuAsync();
        Task<Product> GetProductAsync(Guid id);
        Task<Response> RegisterProductAsync(RegisterProductRequest request);
    }
}