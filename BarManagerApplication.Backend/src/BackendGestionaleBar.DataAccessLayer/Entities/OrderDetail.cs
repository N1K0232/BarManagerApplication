using BackendGestionaleBar.DataAccessLayer.Entities.Common;

namespace BackendGestionaleBar.DataAccessLayer.Entities;

public class OrderDetail : BaseEntity
{
    public virtual Order Order { get; set; }

    public virtual Product Product { get; set; }

    public Guid IdOrder { get; set; }

    public Guid IdProduct { get; set; }
}