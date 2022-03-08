using BackendGestionaleBar.Helpers;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public sealed partial class Database
    {
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

        /// <summary>
        /// creates the connection and tests it
        /// </summary>
        /// <param name="connectionStringHash"></param>
        private void CreateConnection(string connectionStringHash)
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
            catch (InvalidOperationException)
            {
                throw;
            }
        }
    }
}