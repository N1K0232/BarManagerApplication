using BackendGestionaleBar.DataAccessLayer.Entities.Common;

namespace BackendGestionaleBar.DataAccessLayer.Entities;

public class Product : DeletableEntity
{
    public Product() : base()
    {
    }

    public Guid CategoryId { get; set; }

    public string Name { get; set; }

    public DateTime ExpirationDate { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public Category Category { get; set; }

    public List<OrderDetail> OrderDetails { get; set; }
}