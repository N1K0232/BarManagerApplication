using BackendGestionaleBar.DataAccessLayer.Entities.Common;

namespace BackendGestionaleBar.DataAccessLayer.Entities
{
    public class Category : BaseEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}