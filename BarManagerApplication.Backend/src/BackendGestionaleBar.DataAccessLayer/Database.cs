using BackendGestionaleBar.DataAccessLayer.Views;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BackendGestionaleBar.DataAccessLayer;

public sealed class Database : IDatabase
{
	private readonly string connectionString;

	private SqlConnection connection;
	private SqlCommand command;
	private SqlDataReader dataReader;

	private bool disposed;

	public Database(string connectionString)
	{
		this.connectionString = connectionString;

		connection = null;
		command = null;
		dataReader = null;

		disposed = false;
	}

	public async Task<List<Menu>> GetMenuAsync()
	{
		ThrowIfDisposed();

		dataReader = await GetTableAsync("Menu").ConfigureAwait(false);
		List<Menu> result;

		if (dataReader == null)
		{
			result = null;
		}
		else
		{
			result = new List<Menu>();

			while (dataReader.Read())
			{
				Menu menu = new()
				{
					Product = Convert.ToString(dataReader["Product"]),
					Category = Convert.ToString(dataReader["Category"]),
					Price = Convert.ToDecimal(dataReader["Price"]),
					Quantity = Convert.ToInt32(dataReader["Quantity"])
				};

				result.Add(menu);
			}
		}

		return result;
	}
	private async Task<SqlDataReader> GetTableAsync(string tableName)
	{
		SqlDataReader reader;

		try
		{
			await connection.OpenAsync().ConfigureAwait(false);
			command = connection.CreateCommand();
			command.CommandText = $"SELECT * FROM {tableName}";
			reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
			await connection.CloseAsync().ConfigureAwait(false);
		}
		catch (SqlException)
		{
			reader = null;
		}
		catch (InvalidOperationException)
		{
			reader = null;
		}

		return reader;
	}

	internal void InstanceConnection()
	{
		ThrowIfDisposed();

		Exception e = null;
		SqlConnection value = new(connectionString);

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

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
	private void Dispose(bool disposing)
	{
		if (disposing && !disposed)
		{
			if (connection != null)
			{
				if (connection.State == ConnectionState.Open)
				{
					connection.Close();
				}

				connection.Dispose();
				connection = null;
			}

			if (command != null)
			{
				command.Dispose();
				command = null;
			}

			if (dataReader != null)
			{
				dataReader.Dispose();
				dataReader = null;
			}

			disposed = true;
		}
	}
	private void ThrowIfDisposed()
	{
		if (disposed)
		{
			Type currentType = typeof(Database);
			throw new ObjectDisposedException(currentType.FullName);
		}
	}
}