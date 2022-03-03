using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.StartupTasks
{
    public class DatabaseStartupTask : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public DatabaseStartupTask(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            bool exists = await DatabaseExists();
            if (!exists)
            {
                await Create();
            }
        }

        private async Task<bool> DatabaseExists()
        {
            bool exists = false;
            using var scope = serviceProvider.CreateScope();
            string cmdText = "SELECT * FROM master.dbo WHERE name='dbGestionaleBar'";

            try
            {
                SqlConnection connection = scope.ServiceProvider.GetRequiredService<SqlConnection>();
                await connection.OpenAsync();
                SqlCommand command = new(cmdText, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                reader.Dispose();
                command.Dispose();
                await connection.CloseAsync();
                connection.Dispose();
                exists = reader.HasRows;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            catch (InvalidCastException ex)
            {
                throw ex;
            }
            catch (IOException ex)
            {
                throw ex;
            }

            return exists;
        }

        private async Task Create()
        {
            await CreateDatabase();
            //await CreateTables();
        }

        private async Task CreateDatabase()
        {
            using var scope = serviceProvider.CreateScope();
            SqlConnection connection = scope.ServiceProvider.GetRequiredService<SqlConnection>();

            try
            {
                await connection.OpenAsync();
                SqlCommand command = new("CREATE DATABASE dbGestionaleBar", connection);
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
            catch (SqlException ex)
            {
            }
            catch (InvalidOperationException ex)
            {
            }
            catch (IOException ex)
            {

            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}