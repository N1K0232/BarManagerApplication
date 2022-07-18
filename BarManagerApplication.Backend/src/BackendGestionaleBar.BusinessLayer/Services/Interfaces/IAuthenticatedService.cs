using BackendGestionaleBar.Shared.Models;

namespace BackendGestionaleBar.BusinessLayer.Services.Interfaces;
public interface IAuthenticatedService
{
    Task<User> GetUserAsync();
}