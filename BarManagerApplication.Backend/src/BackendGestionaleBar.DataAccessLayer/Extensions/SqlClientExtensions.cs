using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Extensions
{
    public static class SqlClientExtensions
    {
        public static Task<int> FillAsync(this SqlDataAdapter adapter, DataTable dataTable)
        {
            int result = adapter.Fill(dataTable);
            return Task.FromResult(result);
        }
    }
}