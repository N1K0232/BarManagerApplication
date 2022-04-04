using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public interface IOrderService
    {
        Task DeleteAsync();
        Task DeleteAsync(Guid id);
        Task<Order> GetOrderAsync(Guid id);
        Task<Order> SaveAsync(SaveOrderRequest request);
    }
}