using BackendGestionaleBar.Shared.Common;
using BackendGestionaleBar.Shared.Enums;
using BackendGestionaleBar.Shared.Models;

namespace BackendGestionaleBar.Shared.Requests;

public class SaveOrderRequest : BaseRequestObject
{
    public string Umbrella { get; set; }

    public int OrderedQuantity { get; set; }

    public OrderStatus? Status { get; set; }

    public IEnumerable<Product> Products { get; set; }
}