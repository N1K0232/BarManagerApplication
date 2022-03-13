using BackendGestionaleBar.DataAccessLayer.Extensions;
using BackendGestionaleBar.DataAccessLayer.Internal;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    /// <summary>
    /// creates a connection with the database
    /// this class is needed for getting the views in the database
    /// </summary>
    public sealed partial class Database : IDatabase
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;

        /// <summary>
        /// creates a new instance of the <see cref="Database"/>
        /// class
        /// </summary>
        public Database()
        {
            connection = null;
            command = null;
            adapter = null;
        }

        ~Database()
        {
            Dispose(false);
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
            catch (SqlException)
            {
                dataTable = null;
            }
            catch (InvalidOperationException)
            {
                dataTable = null;
            }

            return dataTable;
        }
    }
}