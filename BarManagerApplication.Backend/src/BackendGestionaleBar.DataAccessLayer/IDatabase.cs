using System;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer
{
    public interface IDatabase : IDisposable
    {
        Task<DataTable> GetClienteAsync(Guid idCliente);
    }
}