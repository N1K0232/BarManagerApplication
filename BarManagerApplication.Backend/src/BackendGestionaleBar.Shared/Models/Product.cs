using BackendGestionaleBar.Shared.Models.Common;

namespace BackendGestionaleBar.Shared.Models
{
    public class Product : BaseModel
    {
        public Category Category { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal Price { get; set; }
    }
}