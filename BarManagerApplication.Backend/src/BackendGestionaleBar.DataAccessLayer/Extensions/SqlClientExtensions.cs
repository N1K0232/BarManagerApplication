using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Extensions
{
    public static class SqlClientExtensions
    {
        public static Task<int> FillAsync(this SqlDataAdapter adapter, DataTable dataTable)
        {
            int result = adapter.Fill(dataTable);
            return Task.FromResult(result);
        }

        public static void AddInput(this SqlCommand command, object value)
        {
            SqlParameter parameter = command.CreateParameter();
            parameter.Value = value;
            parameter.Direction = ParameterDirection.Input;
            parameter.IsNullable = false;

            if (value is Guid)
            {
                parameter.SqlDbType = SqlDbType.UniqueIdentifier;
            }
            if (value is string)
            {
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Size = value.ToString().Length;
            }
            if (value is int)
            {
                parameter.SqlDbType = SqlDbType.Int;
            }
            if (value is decimal)
            {
                parameter.SqlDbType = SqlDbType.Decimal;
            }
            if (value is DateTime)
            {
                parameter.SqlDbType = SqlDbType.Date;
            }
            if (value is TimeSpan)
            {
                parameter.SqlDbType = SqlDbType.Time;
            }
            if (value is bool)
            {
                parameter.SqlDbType = SqlDbType.Bit;
            }

            command.Parameters.Add(parameter);
        }
    }
}