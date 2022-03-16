using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    /// <summary>
    /// represents a connection to the sql server database
    /// </summary>
    public interface IDatabase : IDisposable
    {
        /// <summary>
        /// gets or sets the connection to the database
        /// </summary>
        /// <exception cref="SqlException">can't connect to the database</exception>
        /// <exception cref="InvalidOperationException">can't connect to the database</exception>
        SqlConnection Connection { get; set; }

        /// <summary>
        /// gets all the row of the menu view in the database
        /// </summary>
        /// <returns></returns>
        Task<DataTable> GetMenuAsync();
    }
}