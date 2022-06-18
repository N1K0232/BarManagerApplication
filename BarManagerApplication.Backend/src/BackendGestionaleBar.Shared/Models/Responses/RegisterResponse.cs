namespace BackendGestionaleBar.Shared.Models.Responses;

public class RegisterResponse
{
    public RegisterResponse(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors;
    }

    public bool Succeeded { get; }
    public IEnumerable<string> Errors { get; }
}