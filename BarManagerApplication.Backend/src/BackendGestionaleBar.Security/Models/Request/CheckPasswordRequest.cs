namespace BackendGestionaleBar.Security.Models.Request
{
    public class CheckPasswordRequest
    {
        public CheckPasswordRequest(string hash, string password)
        {
            Hash = hash;
            Password = password;
        }

        public string Hash { get; }
        public string Password { get; }
    }
}