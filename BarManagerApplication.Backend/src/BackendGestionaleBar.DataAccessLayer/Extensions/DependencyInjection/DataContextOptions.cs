using System;
using System.Text;

namespace BackendGestionaleBar.DataAccessLayer.Extensions.DependencyInjection
{
    public class DataContextOptions
    {
        private string connectionString = "";

        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ConnectionString_Set(value);
                }
            }
        }

        private void ConnectionString_Set(string value)
        {
            byte[] bytes = Convert.FromBase64String(value);
            connectionString = Encoding.UTF8.GetString(bytes);
        }
    }
}