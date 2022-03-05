using System;

namespace BackendGestionaleBar.DataAccessLayer.Entities
{
    public class Credenziali
    {
        public Guid IdUtente { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime DataRegistrazione { get; set; }

        public DateTime? DataCambioPassword { get; set; }

        public DateTime? DataUltimoAccesso { get; set; }
    }
}