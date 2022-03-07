using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Extensions
{
    public static class SqlClientExtensions
    {
        public static Task<int> FillAsync(this SqlDataAdapter adapter, DataTable dataTable)
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

            return Task.FromResult(result);
        }
    }
}