using System;

namespace BackendGestionaleBar.Shared.Models.Requests
{
    public class RegisterProductRequest
    {
        public Guid IdCategory { get; set; }

        public string Name { get; set; }

        public decimal? Price { get; set; }
    }
}