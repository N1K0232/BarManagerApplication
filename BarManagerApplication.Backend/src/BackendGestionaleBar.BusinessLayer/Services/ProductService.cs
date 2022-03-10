using BackendGestionaleBar.DataAccessLayer.Clients;
using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;
        private readonly IDatabase database;

        public ProductService(ApplicationDbContext context, IDatabase database)
        {
            this.context = context;
            this.database = database;
        }

        public async Task<Product> GetProductAsync(Guid id) => await GetProductInternalAsync(id);

        public async Task<DataTable> GetMenuAsync()
        {
            DataTable dataTable = await database.GetMenuAsync();
            return dataTable;
        }

        public async Task<Response> RegisterProductAsync(RegisterProductRequest request)
        {
            var category = await context.Categories.FindAsync(request.IdCategory.Value);

            var product = new Product
            {
                IdCategory = request.IdCategory.Value,
                Category = category,
                Name = request.Name,
                Price = request.Price.Value
            };

            try
            {
                await context.Products.AddAsync(product);
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

        private async Task<Product> GetProductInternalAsync(Guid id)
        {
            var product = await context.Products.FindAsync(id);
            return product;
        }
    }
}