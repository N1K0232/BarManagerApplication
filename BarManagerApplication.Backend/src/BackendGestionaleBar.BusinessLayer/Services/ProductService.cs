using AutoMapper;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ApplicationProduct = BackendGestionaleBar.DataAccessLayer.Entities.Product;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IDataContext dataContext;
        private readonly IDatabase database;
        private readonly IMapper mapper;

        public ProductService(IDataContext dataContext, IDatabase database, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.database = database;
            this.mapper = mapper;
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await dataContext.GetAsync<ApplicationProduct>(id);
            if (product != null)
            {
                dataContext.Delete(product);
                await dataContext.SaveAsync();
            }
        }

        public async Task DeleteProductsAsync()
        {
            var products = await dataContext.GetData<ApplicationProduct>()
                .Where(p => p.ExpirationDate < DateTime.UtcNow)
                .ToListAsync();

            if (products != null)
            {
                dataContext.Delete(products);
                await dataContext.SaveAsync();
            }
        }
        public async Task<DataTable> GetMenuAsync() => await database.GetMenuAsync();
        public async Task<Product> GetProductAsync(Guid id)
        {
            return await Task.FromResult(new Product());
        }
        public async Task<Product> SaveProductAsync(SaveProductRequest request)
        {
            var dbProduct = request.Id != null ?
                await dataContext.GetAsync<ApplicationProduct>(request.Id.Value) :
                null;

            if (dbProduct == null)
            {
                dbProduct = mapper.Map<ApplicationProduct>(request);
                dbProduct.CreatedDate = DateTime.UtcNow;
                dataContext.Insert(dbProduct);
            }
            else
            {
                dbProduct.LastModifiedDate = DateTime.UtcNow;
            }

            await dataContext.SaveAsync();

            var savedProduct = mapper.Map<Product>(dbProduct);
            return savedProduct;
        }
    }
}