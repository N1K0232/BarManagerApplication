using BackendGestionaleBar.Shared.Common;

namespace BackendGestionaleBar.Shared.Models;

public class User : BaseObject
{
    public string Name { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public string PhoneNumber { get; set; }
}