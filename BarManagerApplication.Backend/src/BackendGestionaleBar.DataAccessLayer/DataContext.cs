using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.DataAccessLayer.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

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

        public async Task<Category> GetCategoryAsync(Guid id)
        {
            var dataTable = await GetTableAsync("Categories", id);
            var category = dataTable == null ? null : new Category
            {
                Id = id,
                Name = Convert.ToString(dataTable.Rows[0]["Name"]),
                Description = Convert.ToString(dataTable.Rows[0]["Description"])
            };

            return category;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            bool result;

            try
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Products WHERE Id=@Id";
                using var command = new SqlCommand(query, connection);
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                result = true;
            }
            catch (SqlException)
            {
                result = false;
            }
            catch (InvalidOperationException)
            {
                result = false;
            }

            return result;
        }
        public async Task<Product> GetProductAsync(Guid id)
        {
            var dataTable = await GetTableAsync("Products", id);
            var product = dataTable == null ? null : new Product
            {
                Id = Guid.Parse(Convert.ToString(dataTable.Rows[0]["Id"])),
                IdCategory = Guid.Parse(Convert.ToString(dataTable.Rows[0]["IdCategory"])),
                Name = Convert.ToString(dataTable.Rows[0]["Name"]),
                Price = Convert.ToDecimal(dataTable.Rows[0]["Price"]),
                ExpirationDate = Convert.ToDateTime(dataTable.Rows[0]["ExpirationDate"]),
                Quantity = Convert.ToInt32(dataTable.Rows[0]["Quantity"])
            };

            return product;
        }
        public async Task<bool> RegisterProductAsync(Product product)
        {
            bool result;
            string query;

            try
            {
                query = "";
                query += "INSERT INTO Products(Id,IdCategory,Name,Price,Quantity,ExpirationDate)";
                query += "VALUES(@Id,@IdCategory,@Name,@Price,@Quantity,@ExpirationDate)";
                await connection.OpenAsync();
                using var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("Id", product.Id));
                command.Parameters.Add(new SqlParameter("IdCategory", product.IdCategory));
                command.Parameters.Add(new SqlParameter("Name", product.Name));
                command.Parameters.Add(new SqlParameter("Price", product.Price));
                command.Parameters.Add(new SqlParameter("Quantity", product.Quantity));
                command.Parameters.Add(new SqlParameter("ExpirationDate", product.ExpirationDate));
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                result = true;
            }
            catch (SqlException)
            {
                result = false;
            }
            catch (InvalidOperationException)
            {
                result = false;
            }

            return result;
        }

        public async Task<DataTable> GetMenuAsync() => await GetTableAsync("ViewMenu");

        private async Task<DataTable> GetTableAsync(string tableName)
        {
            DataTable dataTable;

            try
            {
                await connection.OpenAsync();
                using var command = new SqlCommand($"SELECT * FROM {tableName}", connection);
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
        private async Task<DataTable> GetTableAsync(string tableName, Guid id)
        {
            DataTable dataTable;

            try
            {
                await connection.OpenAsync();
                using var command = new SqlCommand($"SELECT * FROM {tableName} WHERE Id=@Id", connection);
                command.Parameters.Add(new SqlParameter("Id", id));
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