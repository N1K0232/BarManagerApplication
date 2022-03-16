using BackendGestionaleBar.DataAccessLayer.Clients;
using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ApplicationCategory = BackendGestionaleBar.Shared.Models.Category;
using ApplicationProduct = BackendGestionaleBar.Shared.Models.Product;

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

        public async Task<ApplicationProduct> GetProductAsync(Guid id)
        {
            var dbProduct = await GetProductInternalAsync(id);
            var dbCategory = await GetCategoryInternalAsync(dbProduct.IdCategory);
            var category = new ApplicationCategory
            {
                Name = dbCategory.Name,
                Description = dbCategory.Description
            };

            var product = new ApplicationProduct
            {
                Category = category,
                Name = dbProduct.Name,
                Price = dbProduct.Price
            };

            return product;
        }

        public async Task<DataTable> GetMenuAsync() => await database.GetMenuAsync();

        public async Task<Response> RegisterProductAsync(RegisterProductRequest request)
        {
            var category = await GetCategoryInternalAsync(request.IdCategory);

            var product = new Product
            {
                IdCategory = request.IdCategory,
                Category = category,
                Name = request.Name,
                Price = request.Price.Value
            };

            try
            {
                dataContext.Insert(product);
                await dataContext.SaveAsync();
                return new Response
                {
                    Succedeed = true,
                    Errors = null
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    Succedeed = false,
                    Errors = new List<string>
                    {
                        "Errors during registration"
                    }
                };
            }
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await GetProductInternalAsync(id);
            if (product == null)
            {
                return false;
            }

            dataContext.Delete(product);
            await dataContext.SaveAsync();
            return true;
        }

        private async Task<Product> GetProductInternalAsync(Guid id)
        {
            var product = await dataContext.GetAsync<Product>(id);
            return product;
        }

        private async Task<Category> GetCategoryInternalAsync(Guid id)
        {
            var category = await dataContext.GetAsync<Category>(id);
            return category;
        }
    }
}