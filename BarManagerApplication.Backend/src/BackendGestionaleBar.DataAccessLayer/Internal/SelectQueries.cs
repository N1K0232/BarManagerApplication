using System.Text;

namespace BackendGestionaleBar.DataAccessLayer.Internal
{
    internal static class SelectQueries
    {
        public static string GetCliente()
        {
            StringBuilder builder = new();
            builder.AppendLine("SELECT * FROM Clienti");
            builder.AppendLine("WHERE IdCliente=@IdCliente");
            return builder.ToString();
        }
    }
}