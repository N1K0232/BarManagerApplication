using BackendGestionaleBar.DataAccessLayer.Extensions;
using BackendGestionaleBar.DataAccessLayer.Internal;
using BackendGestionaleBar.Helpers;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    internal sealed partial class Database : IDatabase
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;

        public Database(string connectionStringHash)
        {
            InstanceConnection(connectionStringHash);
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Dispose();
                }
                if (command != null)
                {
                    command.Dispose();
                }
                if (adapter != null)
                {
                    adapter.Dispose();
                }
            }
        }
        private void InstanceConnection(string connectionStringHash)
        {
            string connectionString = StringConverter.GetString(connectionStringHash);
            connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                connection.Close();
            }
            catch (SqlException)
            {
                throw;
            }
        }
    }
}