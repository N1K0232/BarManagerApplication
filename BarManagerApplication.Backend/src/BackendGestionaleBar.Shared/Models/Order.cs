using BackendGestionaleBar.Shared.Common;
using BackendGestionaleBar.Shared.Enums;

namespace BackendGestionaleBar.Shared.Models;

public class Order : BaseObject
{
    public User User { get; set; }

    public DateTime OrderDate { get; set; }

    public TimeSpan OrderTime { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public decimal TotalPrice { get; set; }
}