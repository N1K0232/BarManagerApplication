using BackendGestionaleBar.Shared.Common;

namespace BackendGestionaleBar.Shared.Requests;

public class SaveCategoryRequest : BaseRequestObject
{
    public string Name { get; set; }

    public string Description { get; set; }
}