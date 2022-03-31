using BackendGestionaleBar.Shared.Models.Common;
using System;

namespace BackendGestionaleBar.Shared.Models
{
    public class Product : BaseModel
    {
        public Guid IdCategory { get; set; }

        public Category Category { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal Price { get; set; }
    }
}