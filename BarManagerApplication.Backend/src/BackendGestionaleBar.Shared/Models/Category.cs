using BackendGestionaleBar.Shared.Models.Common;

namespace BackendGestionaleBar.Shared.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}