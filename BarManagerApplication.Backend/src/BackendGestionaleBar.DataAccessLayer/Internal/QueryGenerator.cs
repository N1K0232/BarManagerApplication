using System.Text;

namespace BackendGestionaleBar.DataAccessLayer.Internal
{
    internal static class QueryGenerator
    {
        public static string GetCredenziali()
        {
            StringBuilder sb = new();
            sb.AppendLine("SELECT Password FROM Credenziali");
            sb.AppendLine("WHERE Email=@Email");
            return sb.ToString();
        }

        public static string GetCliente()
        {
            StringBuilder sb = new();
            sb.AppendLine("SELECT * FROM Clienti");
            sb.AppendLine("WHERE IdCliente=@IdCliente");
            return sb.ToString();
        }
        public static string InsertCliente()
        {
            StringBuilder sb = new();
            sb.AppendLine("INSERT INTO Clienti(IdCliente,Nome,Cognome,DataNascita,Telefono)");
            sb.AppendLine("VALUES(@IdCliente,@Nome,@Cognome,@DataNascita,@Telefono)");
            return sb.ToString();
        }
    }
}