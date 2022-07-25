using BackendGestionaleBar.DataAccessLayer.Entities.Common;

namespace BackendGestionaleBar.DataAccessLayer.Entities;

public sealed class Category : BaseEntity
{
    public Category() : base()
    {
    }

    public string Name { get; set; }

    public string Description { get; set; }

    public List<Product> Products { get; set; }
}