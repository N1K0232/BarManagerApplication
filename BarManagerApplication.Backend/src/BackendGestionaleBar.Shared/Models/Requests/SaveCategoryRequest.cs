using BackendGestionaleBar.Shared.Models.Common;

namespace BackendGestionaleBar.Shared.Models.Requests;

public class SaveCategoryRequest : BaseRequestObject
{
    public string Name { get; set; }

    public string Description { get; set; }
}