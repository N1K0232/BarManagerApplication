using System;

namespace BackendGestionaleBar.DataAccessLayer.Entities
{
    public class ProductAllergen
    {
        public int IdAllergen { get; set; }
        public Guid IdProduct { get; set; }
        public virtual Allergen Allergen { get; set; }
        public virtual Product Product { get; set; }
    }
}