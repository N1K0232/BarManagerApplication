using BackendGestionaleBar.DataAccessLayer.Clients;
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
        private readonly IDataContext dataContext;
        private readonly IDatabase database;

        public ProductService(IDataContext dataContext, IDatabase database)
        {
            this.dataContext = dataContext;
            this.database = database;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await dataContext.ReadAsync<Product>(id);
            if (product != null)
            {
                dataContext.Delete(product);
                await dataContext.SaveAsync();
                return true;
            }

            return false;
        }

        public async Task<DataTable> GetMenuAsync() => await database.GetMenuAsync();

        public async Task<Product> GetProductAsync(Guid id)
        {
            var product = await dataContext.ReadAsync<Product>();
            return product;
        }

        public Task<Response> RegisterProductAsync(RegisterProductRequest request)
            => Task.FromResult(new Response());
    }
}