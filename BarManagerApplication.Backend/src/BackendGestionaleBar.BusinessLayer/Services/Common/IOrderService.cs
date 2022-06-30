﻿using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Requests;

namespace BackendGestionaleBar.BusinessLayer.Services.Common;
public interface IOrderService
{
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Order>> GetAsync();
    Task<decimal> GetTotalPriceAsync(DateTime orderDate);
    Task<Order> SaveAsync(SaveOrderRequest request);
}