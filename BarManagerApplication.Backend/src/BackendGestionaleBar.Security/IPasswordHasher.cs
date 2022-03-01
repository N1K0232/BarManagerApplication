using BackendGestionaleBar.Security.Models;

namespace BackendGestionaleBar.Security
{
    public interface IPasswordHasher
    {
        CheckResult Check(string hash, string password);
        string Hash(string password);
    }
}