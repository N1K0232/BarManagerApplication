using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Requests;

namespace BackendGestionaleBar.BusinessLayer.Services.Interfaces;

public interface IUmbrellaService
{
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Umbrella>> GetAsync(string coordinates);
    Task<Umbrella> SaveAsync(SaveUmbrellaRequest request);
}