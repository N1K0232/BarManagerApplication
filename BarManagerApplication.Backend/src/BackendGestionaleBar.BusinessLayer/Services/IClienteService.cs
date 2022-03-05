using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public interface IClienteService
    {
        Task<Response> RegisterClienteAsync(RegisterClienteRequest request);
    }
}