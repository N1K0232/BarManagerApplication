using BackendGestionaleBar.Shared.Common;

namespace BackendGestionaleBar.Shared.Models;

public class User : BaseObject
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public string PhoneNumber { get; set; }
}