using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using System.Collections.Generic;

namespace BackendGestionaleBar.DataAccessLayer.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}