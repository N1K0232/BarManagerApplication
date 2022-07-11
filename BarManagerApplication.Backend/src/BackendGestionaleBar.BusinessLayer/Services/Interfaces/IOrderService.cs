using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Requests;

namespace BackendGestionaleBar.BusinessLayer.Services.Interfaces;
public interface IOrderService
{
    Task DeleteAsync(Guid? id);
    Task<IEnumerable<Order>> GetAsync();
    Task<decimal> GetTotalPriceAsync();
    Task<Order> GetYourOrderAsync();
    Task<Order> SaveAsync(SaveOrderRequest request);
}