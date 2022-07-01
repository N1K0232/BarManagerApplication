namespace BackendGestionaleBar.DataAccessLayer.Entities;

public class OrderDetail
{
    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public decimal Price { get; set; }

    public int OrderedQuantity { get; set; }

    public Order Order { get; set; }

    public Product Product { get; set; }
}