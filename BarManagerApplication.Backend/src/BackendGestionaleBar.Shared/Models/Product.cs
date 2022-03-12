namespace BackendGestionaleBar.Shared.Models
{
    public class Product
    {
        public Category Category { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}