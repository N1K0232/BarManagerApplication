using BackendGestionaleBar.DataAccessLayer.Extensions;
using BackendGestionaleBar.DataAccessLayer.Internal;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public sealed partial class Database : IDatabase
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;

        public Database(string connectionStringHash)
        {
            CreateConnection(connectionStringHash);
        }

        public async Task<DataTable> GetMenuAsync()
        {
            DataTable dataTable;
            string query = QueryGenerator.GetMenu();

            try
            {
                await connection.OpenAsync();
                command = new SqlCommand(query, connection);
                adapter = new SqlDataAdapter(command);
                dataTable = new DataTable();
                await adapter.FillAsync(dataTable);
                await connection.CloseAsync();
            }
            catch (Exception)
            {
                dataTable = null;
            }

            return dataTable;
        }
    }
}