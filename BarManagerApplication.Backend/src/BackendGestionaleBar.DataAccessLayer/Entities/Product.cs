using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using System;

namespace BackendGestionaleBar.DataAccessLayer.Entities
{
    public class Product : BaseEntity
    {
        public Guid IdCategory { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public virtual Category Category { get; set; }
    }
}