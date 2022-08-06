using BackendGestionaleBar.DataAccessLayer.Views;

namespace BackendGestionaleBar.DataAccessLayer;

public interface IDatabase : IDisposable
{
    Task<List<Menu>> GetMenuAsync();
}