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
        SqlConnection connection = null;
        SqlCommand command = null;
        SqlDataAdapter adapter = null;

        /// <summary>
        /// creates a new instance of the Database class
        /// </summary>
        /// <param name="connectionStringHash">the encoded connection string</param>
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