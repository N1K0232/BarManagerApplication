using System;

namespace BackendGestionaleBar.Shared.Models.Requests
{
    public class RegisterProductRequest
    {
        public Guid IdCategory { get; set; }

        public string Name { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public int Quantity { get; set; }

        public decimal? Price { get; set; }
    }
}