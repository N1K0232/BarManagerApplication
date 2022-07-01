using BackendGestionaleBar.Shared.Common;
using BackendGestionaleBar.Shared.Enums;

namespace BackendGestionaleBar.Shared.Requests;

public class SaveOrderRequest : BaseRequestObject
{
    public int OrderedQuantity { get; set; }

    public OrderStatus? Status { get; set; }

    public IEnumerable<Guid> ProductIds { get; set; }
}