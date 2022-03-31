using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BackendGestionaleBar.DataAccessLayer
{
    public class DataContext : IDataContext
    {
        private SqlConnection connection;

        public DataContext()
        {
            connection = null;
        }

        public SqlConnection Connection
        {
            get
            {
                return connection;
            }
            set
            {
                Exception e = null;

                if (value != null)
                {
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
                }
                else
                {
                    e = new ArgumentNullException(nameof(value), "Connection can't be null");
                }

                if (e != null)
                {
                    throw e;
                }
                else
                {
                    connection = value;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
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