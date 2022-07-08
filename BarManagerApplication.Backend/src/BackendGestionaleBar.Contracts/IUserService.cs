namespace BackendGestionaleBar.Contracts;

public interface IUserService
{
    Guid GetId();
    string GetUmbrella();
    string GetUsername();
}