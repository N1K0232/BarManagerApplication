using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public interface IProductService
    {
        Task<DataTable> GetMenuAsync();
        Task<Product> GetProductAsync(Guid id);
        Task<Response> RegisterProductAsync(RegisterProductRequest request);
    }
}
