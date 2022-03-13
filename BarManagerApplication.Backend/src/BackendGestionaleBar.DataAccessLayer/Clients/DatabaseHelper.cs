using BackendGestionaleBar.Helpers;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public sealed partial class Database
    {
        public SqlConnection Connection
        {
            get
            {
                return connection;
            }
            set
            {
                Exception e = null;

                try
                {
                    value.Open();
                    value.Close();
                }
                catch (SqlException ex)
                {
                    e = ex;
                }
                catch (InvalidOperationException ex)
                {
                    e = ex;
                }

                if (e == null && value != Connection)
                {
                    connection = value;
                }
                else
                {
                    throw e;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// disposes the connection, the command and the adapter
        /// if the disposing parameter is true
        /// </summary>
        /// <param name="disposing">true if the resources should be disposed otherwise false</param>
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
        /// <param name="connectionStringHash">the encoded connection string</param>
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