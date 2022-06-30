using BackendGestionaleBar.DataAccessLayer.Entities.Common;

namespace BackendGestionaleBar.DataAccessLayer.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public List<Product> Products { get; set; }
}