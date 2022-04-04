using Microsoft.Data.SqlClient;
using System.Data;

namespace BackendGestionaleBar.DataAccessLayer.Extensions
{
    public static class SqlClientExtensions
    {
        public static Task<int> FillAsync(this SqlDataAdapter adapter, DataTable dataTable)
        {
            int rows;

            try
            {
                rows = adapter.Fill(dataTable);
            }
            catch (Exception)
            {
                rows = 0;
            }

            return Task.FromResult(rows);
        }
    }
}