using Microsoft.Data.SqlClient;
using System.Data;

namespace BackendGestionaleBar.DataAccessLayer
{
    public interface IDatabase : IDisposable
    {
        SqlConnection Connection { get; set; }

        Task<DataTable> GetMenuAsync();
        Task<decimal> GetPriceAsync(Guid idOrder);
    }
}