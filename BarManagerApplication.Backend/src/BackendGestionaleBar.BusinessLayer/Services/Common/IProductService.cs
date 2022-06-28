﻿using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Requests;

namespace BackendGestionaleBar.BusinessLayer.Services.Common;
public interface IProductService
{
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Product>> GetAsync(string name);
    Task<Product> SaveAsync(SaveProductRequest request);
}
