using AutoMapper;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;
using System;
using System.Data;
using System.Threading.Tasks;
using ApplicationProduct = BackendGestionaleBar.DataAccessLayer.Entities.Product;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IDataContext dataContext;
        private readonly IMapper mapper;

        public ProductService(IDataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<bool> DeleteProductAsync(Guid id) => await dataContext.DeleteProductAsync(id);

        public async Task<DataTable> GetMenuAsync() => await dataContext.GetMenuAsync();

        public async Task<Product> GetProductAsync(Guid id)
        {
            var productEntity = await dataContext.GetProductAsync(id);
            var categoryEntity = await dataContext.GetCategoryAsync(productEntity.IdCategory);

            var product = mapper.Map<Product>(productEntity);
            var category = mapper.Map<Category>(categoryEntity);
            product.Category = category;
            return product;
        }

        public async Task<bool> RegisterProductAsync(RegisterProductRequest request)
        {
            var product = new ApplicationProduct
            {
                Id = Guid.NewGuid(),
                IdCategory = request.IdCategory,
                Name = request.Name,
                Price = request.Price.Value,
                Quantity = request.Quantity,
                ExpirationDate = request.ExpirationDate.Value
            };

            var result = await dataContext.RegisterProductAsync(product);
            return result;
        }
    }
}