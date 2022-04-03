using AutoMapper;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;
using ApplicationCategory = BackendGestionaleBar.DataAccessLayer.Entities.Category;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IDataContext dataContext;
        private readonly IMapper mapper;

        public CategoryService(IDataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<Category> GetCategoryAsync(Guid id)
        {
            var dbCategory = await dataContext.GetAsync<ApplicationCategory>(id);
            var category = mapper.Map<Category>(dbCategory);
            return category;
        }
        public async Task<Category> SaveCategoryAsync(SaveCategoryRequest request)
        {
            var dbCategory = request.Id != null ?
                await dataContext.GetAsync<ApplicationCategory>(request.Id) :
                null;

            if (dbCategory == null)
            {
                dbCategory = mapper.Map<ApplicationCategory>(request);
                dataContext.Insert(dbCategory);
            }
            else
            {
                dbCategory.LastModifiedDate = DateTime.UtcNow;
            }

            await dataContext.SaveAsync();
            return mapper.Map<Category>(dbCategory);
        }
    }
}