namespace BackendGestionaleBar.Shared.Models.Responses
{
    public class SaveOrderRequest
    {
        public Guid? Id { get; set; }

        public Guid IdUser { get; set; }

        public Guid IdProduct { get; set; }
    }
}