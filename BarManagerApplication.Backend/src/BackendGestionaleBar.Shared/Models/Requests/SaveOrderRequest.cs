namespace BackendGestionaleBar.Shared.Models.Requests;

public class SaveOrderRequest
{
    public Guid? Id { get; set; }

    public Guid IdProduct { get; set; }

    public Guid IdUser { get; set; }
}