using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Requests;

namespace BackendGestionaleBar.BusinessLayer.Services.Interfaces;

public interface ICategoryService
{
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Category>> GetAsync(string name);
    Task<Category> SaveAsync(SaveCategoryRequest request);
}