namespace BackendGestionaleBar.Shared.Models.Responses;

public class RegisterResponse
{
    public bool Succeeded { get; set; }

    public IEnumerable<string> Errors { get; set; }
}