namespace BackendGestionaleBar.Shared.Models.Responses;

public class AuthResponse
{
    public AuthResponse(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; }
    public string RefreshToken { get; }
}