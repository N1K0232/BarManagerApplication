using System;

namespace BackendGestionaleBar.DataAccessLayer.Entities
{
    public class Cliente
    {
        public Guid IdCliente { get; set; }

        public string Nome { get; set; }

        public string Cognome { get; set; }

        public DateTime DataNascita { get; set; }

        public string CodiceFiscale { get; set; }

        public string Telefono { get; set; }
    }
}
