using System;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public interface IDatabase : IDisposable
    {
        Task<DataTable> GetMenuAsync();
    }
}
