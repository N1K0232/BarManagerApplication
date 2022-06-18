using BackendGestionaleBar.Shared.Models.Common;

namespace BackendGestionaleBar.Shared.Models.Requests;

public class SaveOrderRequest : BaseRequestObject
{
    public Guid IdProduct { get; set; }

    public Guid IdUser { get; set; }
}