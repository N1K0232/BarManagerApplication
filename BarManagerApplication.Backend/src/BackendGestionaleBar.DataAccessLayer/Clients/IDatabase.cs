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
        SqlConnection Connection { get; set; }

        /// <summary>
        /// gets all the row of the menu view in the database
        /// </summary>
        /// <returns></returns>
        Task<DataTable> GetMenuAsync();
    }
}