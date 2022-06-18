using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;

namespace BackendGestionaleBar.BusinessLayer.Services;

public interface ICategoryService
{
    Task DeleteAsync(Guid? id);
    Task<IEnumerable<Category>> GetAsync(string name);
    Task<Category> SaveAsync(SaveCategoryRequest request);
}