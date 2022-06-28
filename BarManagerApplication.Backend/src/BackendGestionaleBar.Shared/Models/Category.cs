using BackendGestionaleBar.Shared.Common;

namespace BackendGestionaleBar.Shared.Models;

public class Category : BaseObject
{
    public string Name { get; set; }

    public string Description { get; set; }
}