using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.DataAccessLayer.Extensions;
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
        SqlCommand command;
        SqlDataAdapter adapter;

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

        public async Task<DataRow> GetClienteAsync(Guid idCliente)
        {
            string query = QueryGenerator.GetCliente();
            DataRow row = null;
            DataTable dataTable = await GetClienteAsync(idCliente, query);

            if (dataTable != null && dataTable.Rows.Count == 1)
            {
                row = dataTable.Rows[0];
            }

            return row;
        }
        public async Task<int> RegisterClienteAsync(Cliente cliente)
        {
            int result;
            string query = QueryGenerator.InsertCliente();

            try
            {
                await connection.OpenAsync();
                command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("IdCliente", cliente.IdCliente));
                command.Parameters.Add(new SqlParameter("Nome", cliente.Nome));
                command.Parameters.Add(new SqlParameter("Cognome", cliente.Cognome));
                command.Parameters.Add(new SqlParameter("DataNascita", cliente.DataNascita));
                command.Parameters.Add(new SqlParameter("CodiceFiscale", cliente.CodiceFiscale));
                command.Parameters.Add(new SqlParameter("Telefono", cliente.Telefono));
                result = await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();

                command.Dispose();
            }
            catch (SqlException)
            {
                result = -1;
            }
            catch (InvalidOperationException)
            {
                result = -2;
            }

            return result;
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

        private async Task<DataTable> GetClienteAsync(Guid idCliente, string query)
        {
            DataTable dataTable;
            try
            {
                await connection.OpenAsync();
                command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("IdCliente", idCliente));
                adapter = new SqlDataAdapter(command);
                dataTable = new DataTable();
                await adapter.FillAsync(dataTable);
                await connection.CloseAsync();

                adapter.Dispose();
                command.Dispose();
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