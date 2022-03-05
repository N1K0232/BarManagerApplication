using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.DataAccessLayer.Extensions;
using BackendGestionaleBar.DataAccessLayer.Internal;
using BackendGestionaleBar.Security;
using BackendGestionaleBar.Security.Models.Request;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer
{
    public partial class Database : IDatabase, IDisposable
    {
        SqlConnection connection = null;
        SqlCommand command = null;
        SqlDataAdapter adapter = null;

        PasswordHasher hasher = null;

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

        public async Task<bool> LoginAsync(string email, string password)
        {
            DataTable dataTable;

            try
            {
                await connection.OpenAsync();
                command = new SqlCommand(QueryGenerator.GetCredenziali(), connection);
                command.Parameters.Add(new SqlParameter("Email", email));
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

            if (dataTable == null)
            {
                return false;
            }

            string dbPassword = Convert.ToString(dataTable.Rows[0]["Password"]);
            return CheckLogin(dbPassword, password);
        }
        private bool CheckLogin(string dbPassword, string password)
        {
            hasher = new PasswordHasher();
            var request = new CheckPasswordRequest(dbPassword, password);
            var response = hasher.Check(request);
            return response.Verified && !response.NeedsUpgrade;
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
            //try
            //{
            //    await connection.OpenAsync();
            //    command = new SqlCommand("SP_RegisterCliente", connection);
            //    command.AddInput(cliente.IdCliente);
            //    command.AddInput(cliente.Nome);
            //    command.AddInput(cliente.Cognome);
            //    command.AddInput(cliente.DataNascita.Date);
            //    command.AddInput(cliente.CodiceFiscale);
            //    command.AddInput(cliente.Telefono);
            //    command.Parameters.Add(new SqlParameter("@Identity", SqlDbType.Int).Direction = ParameterDirection.Output);
            //    await command.PrepareAsync();
            //    int result = await command.ExecuteNonQueryAsync();
            //    await connection.CloseAsync();
            //    return Convert.ToInt32(command.Parameters["@Identity"]?.Value);
            //}
            //catch (SqlException)
            //{
            //    if (connection.State == ConnectionState.Open)
            //    {
            //        await connection.CloseAsync();
            //    }
            //    return -1;
            //}
            //catch (InvalidOperationException)
            //{
            //    if (connection.State == ConnectionState.Open)
            //    {
            //        await connection.CloseAsync();
            //    }
            //    return -2;
            //}

            int result;
            string query;

            try
            {
                await connection.OpenAsync();
                query = QueryGenerator.InsertCliente();
                command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("IdCliente", cliente.IdCliente));
                command.Parameters.Add(new SqlParameter("Nome", cliente.Nome));
                command.Parameters.Add(new SqlParameter("Cognome", cliente.Cognome));
                command.Parameters.Add(new SqlParameter("DataNascita", cliente.DataNascita));
                command.Parameters.Add(new SqlParameter("Telefono", cliente.Telefono));
                result = await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
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


        public async Task<int> RegisterCredenzialiAsync(Credenziali credenziali, string role)
        {
            int result;
            hasher = new PasswordHasher();
            string query = QueryGenerator.RegisterCredenziali();

            try
            {
                await connection.OpenAsync();
                command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("IdUtente", credenziali.IdUtente));
                command.Parameters.Add(new SqlParameter("Email", credenziali.Email));
                command.Parameters.Add(new SqlParameter("Password", hasher.Hash(credenziali.Password)));
                command.Parameters.Add(new SqlParameter("Role", role));
                command.Parameters.Add(new SqlParameter("DataRegistrazione", credenziali.DataRegistrazione));
                result = await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
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