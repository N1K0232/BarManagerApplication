using BackendGestionaleBar.Shared.Models.Common;

namespace BackendGestionaleBar.Shared.Models.Requests;

public class SaveProductRequest : BaseRequestObject
{
    public string Name { get; set; }

    public string CategoryName { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }
}