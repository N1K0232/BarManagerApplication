namespace BackendGestionaleBar.Shared.Models.Requests
{
    public class RegisterProductRequest
    {
        public int? IdCategory { get; set; }

        public string Name { get; set; }

        public decimal? Price { get; set; }
    }
}