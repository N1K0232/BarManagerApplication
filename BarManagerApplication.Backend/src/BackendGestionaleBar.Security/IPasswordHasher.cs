using BackendGestionaleBar.Security.Models.Request;
using BackendGestionaleBar.Security.Models.Response;

namespace BackendGestionaleBar.Security
{
    public interface IPasswordHasher
    {
        CheckPasswordResponse Check(CheckPasswordRequest request);
        string Hash(string password);
    }
}