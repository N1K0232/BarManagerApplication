﻿using BackendGestionaleBar.DataAccessLayer.Extensions;
using BackendGestionaleBar.DataAccessLayer.Internal;
using BackendGestionaleBar.Security;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer
{
    public partial class Database : IDatabase, IDisposable
    {
        SqlConnection connection;
        PasswordHasher hasher;

        public Database(string[] parameters)
        {
            InstanceConnection(parameters);
        }
        public Database(SqlConnection connection)
        {
            this.connection = connection;
        }

        private void InstanceConnection(string[] parameters)
        {
            //building connection string for azure client
            SqlConnectionStringBuilder builder = new();
            builder.DataSource = parameters[0];
            builder.InitialCatalog = parameters[1];
            builder.UserID = parameters[2];
            builder.Password = parameters[3];

            //instance connection
            connection = new SqlConnection(builder.ConnectionString);

            //testing connection
            try
            {
                connection.Open();
                connection.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetClienteAsync(Guid idCliente)
        {
            DataTable dataTable;

            try
            {
                await connection.OpenAsync();
                using var command = connection.CreateCommand();
                command.CommandText = SelectQueries.GetCliente();
                command.Parameters.Add(new SqlParameter("IdCliente", idCliente));
                using var adapter = new SqlDataAdapter(command);
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