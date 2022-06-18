namespace BackendGestionaleBar.Shared.Models.Requests;

public class SaveCategoryRequest
{
    public Guid? Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}