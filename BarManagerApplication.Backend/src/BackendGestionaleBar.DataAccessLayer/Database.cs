﻿using BackendGestionaleBar.DataAccessLayer.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BackendGestionaleBar.DataAccessLayer
{
    public class Database : IDatabase
    {
        private SqlConnection connection;

        public Database()
        {
            connection = null;
        }

        public SqlConnection Connection
        {
            get => connection;
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

                if (e != null)
                {
                    throw e;
                }

                connection = value;
            }
        }

        public void Dispose()
        {
        }

        public async Task<DataTable> GetMenuAsync()
        {
            DataTable dataTable;

            try
            {
                await connection.OpenAsync();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM ViewMenu";
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
    }
}