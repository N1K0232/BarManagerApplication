using System.Text;

namespace BackendGestionaleBar.DataAccessLayer.Internal
{
    internal static class QueryGenerator
    {
        public static string GetCliente()
        {
            StringBuilder builder = new();
            builder.AppendLine("SELECT * FROM Clienti");
            builder.AppendLine("WHERE IdCliente=@IdCliente");
            return builder.ToString();
        }
        public static string InsertCliente()
        {
            StringBuilder builder = new();
            builder.AppendLine("INSERT INTO Clienti(IdCliente,Nome,Cognome,DataNascita,CodiceFiscale,Telefono)");
            builder.AppendLine("VALUES(@IdCliente,@Nome,@Cognome,@DataNascita,@CodiceFiscale,@Telefono)");
            return builder.ToString();
        }
    }
}