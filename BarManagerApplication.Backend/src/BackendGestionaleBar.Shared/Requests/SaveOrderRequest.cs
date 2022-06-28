using BackendGestionaleBar.Shared.Common;

namespace BackendGestionaleBar.Shared.Requests;

public class SaveOrderRequest : BaseRequestObject
{
    public Guid IdProduct { get; set; }

    public Guid IdUser { get; set; }
}