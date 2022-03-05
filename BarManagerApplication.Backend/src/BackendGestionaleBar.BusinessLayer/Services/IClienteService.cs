using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;
using System;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public interface IClienteService
    {
        Task<Cliente> GetClienteAsync(Guid idCliente);
        Task<Response> RegisterClienteAsync(RegisterClienteRequest request);
    }
}