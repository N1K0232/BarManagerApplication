﻿using System;

namespace BackendGestionaleBar.Shared.Models.Requests
{
    public class RegisterClienteRequest
    {
        public string Nome { get; set; }

        public string Cognome { get; set; }

        public DateTime? DataNascita { get; set; }

        public string CodiceFiscale { get; set; }

        public string Telefono { get; set; }
    }
}