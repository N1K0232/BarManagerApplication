using BackendGestionaleBar.DataAccessLayer.Entities.Common;

namespace BackendGestionaleBar.DataAccessLayer.Entities;

public class Product : BaseEntity
{
    public Guid CategoryId { get; set; }

    public string Name { get; set; }

    public DateTime ExpirationDate { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public Category Category { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }
}