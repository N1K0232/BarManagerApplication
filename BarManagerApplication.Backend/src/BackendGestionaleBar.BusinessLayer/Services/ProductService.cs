using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public class ProductService : IProductService
    {
        public Task<bool> DeleteProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DataTable> GetMenuAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> RegisterProductAsync(RegisterProductRequest request)
        {
            throw new NotImplementedException();
        }
    }
}