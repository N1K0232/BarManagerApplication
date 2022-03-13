using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using static BackendGestionaleBar.Helpers.StringConverter;

namespace BackendGestionaleBar.DataAccessLayer.Internal
{
    internal static class ConnectionHelper
    {
        public static SqlConnection CreateAzureConnection(IConfiguration configuration)
        {
            SqlConnectionStringBuilder builder = new();
            builder.DataSource = GetString(configuration["ConnectionStrings:DataSource"]);
            builder.InitialCatalog = GetString(configuration["ConnectionStrings:InitialCatalog"]);
            builder.UserID = GetString(configuration["ConnectionStrings:DataSource"]);
            builder.Password = GetString(configuration["ConnectionStrings:Password"]);

            SqlConnection connection = new(builder.ConnectionString);
            return connection;
        }
    }
}