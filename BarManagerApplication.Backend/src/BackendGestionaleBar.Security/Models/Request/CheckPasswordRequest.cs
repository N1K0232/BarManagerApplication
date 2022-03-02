namespace BackendGestionaleBar.Security.Models.Request
{
    public class CheckPasswordRequest
    {
        public string Hash { get; set; }
        public string Password { get; set; }
    }
}