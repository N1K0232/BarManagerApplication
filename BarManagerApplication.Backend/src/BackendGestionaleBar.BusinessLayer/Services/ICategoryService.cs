using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryAsync(Guid id);
        Task<Category> SaveCategoryAsync(SaveCategoryRequest request);
    }
}