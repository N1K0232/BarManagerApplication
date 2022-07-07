using BackendGestionaleBar.DataAccessLayer.Entities.Common;

namespace BackendGestionaleBar.DataAccessLayer.Entities;

public class Umbrella : DeletableEntity
{
    public Umbrella() : base()
    {
    }

    public string Coordinates { get; set; }

    public List<Order> Orders { get; set; }
}