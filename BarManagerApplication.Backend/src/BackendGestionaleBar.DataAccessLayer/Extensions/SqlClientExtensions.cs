using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Extensions
{
    public static class SqlClientExtensions
    {
        public static async Task<int> FillAsync(this SqlDataAdapter adapter, DataTable dataTable)
        {
            int result;

            try
            {
                result = adapter.Fill(dataTable);
            }
            catch (InvalidOperationException)
            {
                result = -1;
            }

            return await Task.FromResult(result);
        }
    }
}