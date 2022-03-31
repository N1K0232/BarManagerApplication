using Microsoft.Data.SqlClient;
using System;

namespace BackendGestionaleBar.DataAccessLayer
{
    public interface IDataContext : IDisposable
    {
        SqlConnection Connection { get; set; }
    }
}