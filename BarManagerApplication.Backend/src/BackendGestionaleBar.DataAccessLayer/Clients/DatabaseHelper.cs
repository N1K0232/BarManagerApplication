using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public sealed partial class Database
    {
        /// <summary>
        /// gets or sets the connection to the database
        /// </summary>
        /// <exception cref="SqlException">can't connect to the database</exception>
        /// <exception cref="InvalidOperationException">can't connect to the database</exception>
        public SqlConnection Connection
        {
            get => connection;
            set
            {
                //tests the connection before assign it to
                //the connection field
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

                if (e == null)
                {
                    if (value != Connection)
                    {
                        connection = value;
                    }
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
            if (disposing && connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
        }
    }
}