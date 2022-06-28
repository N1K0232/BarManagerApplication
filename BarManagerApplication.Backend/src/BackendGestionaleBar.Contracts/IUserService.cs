namespace BackendGestionaleBar.Contracts;

public interface IUserService
{
    Guid GetId();
    string GetUsername();
}